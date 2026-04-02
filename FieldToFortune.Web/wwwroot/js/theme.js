// wwwroot/js/theme.js
// Called from Blazor via JSRuntime.InvokeVoidAsync("setTheme", "dark"|"light")

window.setTheme = function (theme) {
    document.body.classList.remove("theme-dark", "theme-light");
    document.body.classList.add("theme-" + theme);
    localStorage.setItem("ftf-theme", theme);
};

// Restore saved theme on page load
(function () {
    const saved = localStorage.getItem("ftf-theme") || "dark";
    document.body.classList.add("theme-" + saved);
})();
