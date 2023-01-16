const RegisterPage = {
  disableButtonRegister() {
    const checkboxAgree = $('#agree-register');
    const btnRegister = $('#btn-register');

    if (!checkboxAgree.prop('checked')) {
      btnRegister.prop('disabled', true);
    }

    checkboxAgree.on('change', () => {
      if (!checkboxAgree.prop('checked')) {
        btnRegister.prop('disabled', true);
      } else {
        btnRegister.prop('disabled', false);
      }
    });
  },

  init() {
    this.disableButtonRegister();
  },
};

RegisterPage.init();
