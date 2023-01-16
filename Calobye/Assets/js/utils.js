const utils = {
  debounce(callback, delay = 1000) {
    let timer;
    return (...args) => {
      if (timer) clearTimeout(timer);

      callback(...args.map(() => undefined), true);
      timer = setTimeout(() => {
        callback(...args, false);
      }, delay);
    };
  },

  formatMoney(amount = 0, decimalCount = 0, decimal = '.', thousands = ',') {
    decimalCount = Math.abs(decimalCount);
    decimalCount = isNaN(decimalCount) ? 2 : decimalCount;

    const negativeSign = amount < 0 ? '-' : '';

    const i = parseInt((amount = Math.abs(Number(amount) || 0).toFixed(decimalCount))).toString();
    const j = i.length > 3 ? i.length % 3 : 0;

    return (
      negativeSign +
      (j ? i.substr(0, j) + thousands : '') +
      i.substr(j).replace(/(\d{3})(?=\d)/g, `$1${thousands}`) +
      (decimalCount
        ? decimal +
          // @ts-ignore
          Math.abs(amount - i)
            .toFixed(decimalCount)
            .slice(2)
        : '')
    );
  },

  uid() {
    return Date.now().toString(36) + Math.floor(Math.pow(10, 12) + Math.random() * 9 * Math.pow(10, 12)).toString(36);
  },


  
};

export default utils;
