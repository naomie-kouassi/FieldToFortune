//Switch theme colors by changing css variables
window.setTheme = function (theme) {
    document.body.classList.remove("theme-dark", "theme-light");
    document.body.classList.add("theme-" + theme);
};

// Restore saved theme on page load
(function () {
    const sessionData = sessionStorage.getItem("ftf-game-session");
    let theme = "light"; // Default theme is light

    if (sessionData) {
        try {
            const gameState = JSON.parse(sessionData);
            if (gameState.IsDarkMode === true) theme = "dark";
        } 
        catch (e) {
            console.error("Failed to parse theme from session", e);
        }
    }
    document.body.classList.remove("theme-dark", "theme-light");
    document.body.classList.add("theme-" + theme);
})();