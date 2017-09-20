$(document).ready(function() {
  // close button in the forgotten.php
  $('#close-login').click(function(event) {
    history.back();

    return false;
  });

  // login button in login.php
  $('#btn-login').click(function(event) {
    var email = $('#email').val();

    if (email.length > 0){
      window.location = 'dashboard.html';
    } else {
      $('#login-error').removeClass('hide');
    }

    return false;
  });
});
