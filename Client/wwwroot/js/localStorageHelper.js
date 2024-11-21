window.localStorageHelper = {
    save: function (key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },
    get: function (key) {
        const value = localStorage.getItem(key);
        return value ? JSON.parse(value) : null;
    },
    remove: function (key) {
        localStorage.removeItem(key);
    }
};
