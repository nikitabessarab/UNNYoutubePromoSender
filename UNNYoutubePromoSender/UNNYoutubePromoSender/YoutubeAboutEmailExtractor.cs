using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace UNNYoutubePromoSender;

/// <summary>
/// Пытается найти публичный контактный email на странице «О канале» YouTube (mailto, раскрытие кнопки, текст).
/// </summary>
public static class YoutubeAboutEmailExtractor
{
    private static readonly Regex EmailRegex = new(
        @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly HashSet<string> BlockedDomains = new(StringComparer.OrdinalIgnoreCase)
    {
        "youtube.com", "youtu.be", "google.com", "google.ru", "gstatic.com",
        "facebook.com", "instagram.com", "twitter.com", "x.com"
    };

    public static string? TryExtract(IWebDriver driver, int timeoutSeconds = 8, CancellationToken cancellationToken = default)
    {
        if (driver is null)
            throw new ArgumentNullException(nameof(driver));

        var deadline = DateTime.UtcNow.AddSeconds(Math.Clamp(timeoutSeconds, 2, 60));

        while (DateTime.UtcNow < deadline)
        {
            cancellationToken.ThrowIfCancellationRequested();

            TryClickRevealEmail(driver);

            foreach (var href in CollectMailtoHrefs(driver))
            {
                var email = ParseMailto(href);
                if (IsPlausibleContactEmail(email))
                    return email;
            }

            var fromText = FirstEmailInVisibleText(driver);
            if (fromText is not null)
                return fromText;

            var fromSource = FirstEmailInPageSource(driver);
            if (fromSource is not null)
                return fromSource;

            Thread.Sleep(350);
        }

        return null;
    }

    private static void TryClickRevealEmail(IWebDriver driver)
    {
        if (driver is not IJavaScriptExecutor js)
            return;

        try
        {
            js.ExecuteScript(
                """
                (function () {
                  const patterns = [
                    /view\s+email/i,
                    /email\s+address/i,
                    /business\s+email/i,
                    /показать\s+адрес/i,
                    /электронн/i,
                    /^e-?mail$/i
                  ];
                  function textOf(el) {
                    return (el.innerText || el.textContent || '').trim();
                  }
                  function walk(root) {
                    if (!root || !root.querySelectorAll) return false;
                    const candidates = root.querySelectorAll(
                      'button, a, tp-yt-paper-button, yt-button-shape, yt-formatted-string');
                    for (const el of candidates) {
                      const t = textOf(el);
                      if (!t || t.length > 140) continue;
                      if (patterns.some(p => p.test(t))) {
                        el.click();
                        return true;
                      }
                    }
                    for (const el of root.querySelectorAll('*')) {
                      if (el.shadowRoot && walk(el.shadowRoot)) return true;
                    }
                    return false;
                  }
                  return walk(document);
                })();
                """);
        }
        catch
        {
            // игнорируем смену DOM / перекрытия
        }
    }

    private static IEnumerable<string> CollectMailtoHrefs(IWebDriver driver)
    {
        if (driver is not IJavaScriptExecutor js)
            yield break;

        object? raw;
        try
        {
            raw = js.ExecuteScript(
                """
                return (function () {
                  const urls = [];
                  function scan(root) {
                    if (!root || !root.querySelectorAll) return;
                    root.querySelectorAll('a[href^="mailto:"]').forEach(function (a) {
                      urls.push(a.getAttribute('href') || '');
                    });
                    root.querySelectorAll('*').forEach(function (el) {
                      if (el.shadowRoot) scan(el.shadowRoot);
                    });
                  }
                  scan(document);
                  return urls;
                })();
                """);
        }
        catch
        {
            yield break;
        }

        foreach (var item in ToObjectEnumerable(raw))
        {
            var s = item?.ToString();
            if (!string.IsNullOrWhiteSpace(s))
                yield return s;
        }
    }

    private static IEnumerable<object?> ToObjectEnumerable(object? raw)
    {
        switch (raw)
        {
            case null:
                yield break;
            case object[] arr:
                foreach (var x in arr)
                    yield return x;
                yield break;
            case System.Collections.IEnumerable e:
                foreach (var x in e)
                    yield return x;
                yield break;
            default:
                yield return raw;
                yield break;
        }
    }

    private static string? ParseMailto(string href)
    {
        if (string.IsNullOrWhiteSpace(href))
            return null;
        var u = href.Trim();
        if (!u.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
            return null;
        var rest = u["mailto:".Length..];
        var q = rest.IndexOf('?', StringComparison.Ordinal);
        if (q >= 0)
            rest = rest[..q];
        try
        {
            rest = Uri.UnescapeDataString(rest);
        }
        catch
        {
            // оставляем как есть
        }

        return string.IsNullOrWhiteSpace(rest) ? null : rest.Trim();
    }

    private static string? FirstEmailInVisibleText(IWebDriver driver)
    {
        string text;
        try
        {
            text = driver.FindElement(By.TagName("body")).Text ?? "";
        }
        catch
        {
            return null;
        }

        foreach (Match m in EmailRegex.Matches(text))
        {
            if (IsPlausibleContactEmail(m.Value))
                return m.Value;
        }

        return null;
    }

    private static string? FirstEmailInPageSource(IWebDriver driver)
    {
        string html;
        try
        {
            html = driver.PageSource ?? "";
        }
        catch
        {
            return null;
        }

        foreach (Match m in EmailRegex.Matches(html))
        {
            if (IsPlausibleContactEmail(m.Value))
                return m.Value;
        }

        return null;
    }

    private static bool IsPlausibleContactEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        email = email.Trim();
        if (email.Length > 254)
            return false;

        var at = email.LastIndexOf('@');
        if (at <= 0 || at == email.Length - 1)
            return false;

        var local = email[..at];
        var domain = email[(at + 1)..];

        if (local.StartsWith("noreply", StringComparison.OrdinalIgnoreCase) ||
            local.StartsWith("no-reply", StringComparison.OrdinalIgnoreCase) ||
            local.Contains("donotreply", StringComparison.OrdinalIgnoreCase))
            return false;

        if (BlockedDomains.Contains(domain))
            return false;

        return true;
    }
}
