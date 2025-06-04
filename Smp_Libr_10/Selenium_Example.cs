using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Smp_Libr_10 {
    public class Selenium_Example {
        private IWebDriver driver;
        private WebDriverWait wait;

        public Selenium_Example() {
            var options = new ChromeOptions();
            options.AddArgument("--headless");  // ヘッドレスモードで実行
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        // ... 既存のコード ...
    }
} 