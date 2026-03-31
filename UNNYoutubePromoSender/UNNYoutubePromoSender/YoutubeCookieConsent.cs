using OpenQA.Selenium;

namespace UNNYoutubePromoSender;

/// <summary>
/// Пытается закрыть баннер согласия (Accept all и аналоги на разных языках).
/// </summary>
public static class YoutubeCookieConsent
{
    public static void TryAcceptAll(IWebDriver driver, int rounds = 5)
    {
        if (driver is not IJavaScriptExecutor js)
            return;

        for (var i = 0; i < rounds; i++)
        {
            try
            {
                js.ExecuteScript(
                    """
                    (function () {
                      const patterns = [
                        /^accept all$/i,
                        /^accept$/i,
                        /accept\s+all/i,
                        /принять\s+вс[её]/i,
                        /^принять$/i,
                        /согласен/i,
                        /alle\s+akzeptieren/i,
                        /tout\s+accepter/i,
                        /aceptar\s+todo/i,
                        /accepteer\s+alles/i,
                        /^agree$/i,
                        /i\s+agree/i
                      ];
                      function textOf(el) {
                        return (el.innerText || el.textContent || '').trim();
                      }
                      function walk(root) {
                        if (!root || !root.querySelectorAll) return false;
                        const candidates = root.querySelectorAll(
                          'button, a, [role="button"], tp-yt-paper-button, yt-button-shape');
                        for (const el of candidates) {
                          const t = textOf(el);
                          if (!t || t.length > 90) continue;
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
                // DOM может перестраиваться
            }

            Thread.Sleep(280);
        }
    }
}
