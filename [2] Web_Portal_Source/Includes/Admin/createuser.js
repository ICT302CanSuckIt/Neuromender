function showAffliction()
{
	if ($("#roleSelection").val() == "5")
	{
		$(".AfflictionData").show();
	}
	else
	{
		$(".AfflictionData").hide();
	}
}



function ValidateUsername( username )
{
	if( username.length == 0 )
	{
		var output = "Please enter a valid username";
		document.getElementById("userNameError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("userNameError").textContent = "";
		return true;
	}
}


function ValidatePassword( password )
{
	if( password.length == 0 )
	{
		var output = "Please enter a valid password";
		document.getElementById("passwordError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("passwordError").textContent = "";
		return true;
	}
}


function ValidateConfirmPassword( password, confPassword )
{
	if( confPassword.length == 0 || (password == confPassword) == false )
	{
		var output = "Password confirmation did not match password";
		document.getElementById("confPasswordError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("confPasswordError").textContent = "";
		return true;
	}
}

function ValidateEmail( email )
{
	var regex = /\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b/;
	if( email.length == 0 || !regex.test(email) )
	{
		var output = "Please enter a valid email";
		document.getElementById("EmailError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("EmailError").textContent = "";
		return true;
	}
}

function ValidateFullName( name )
{
	if( name.length == 0 )
	{
		var output = "Please enter a valid name";
		document.getElementById("fullNameError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("fullNameError").textContent = "";
		return true;
	}
}

function ValidateQuestion( question )
{
	if( question.length == 0 )
	{
		var output = "Please enter a valid security question";
		document.getElementById("questionError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("questionError").textContent = "";
		return true;
	}
}

function ValidateAnswer( answer )
{
	if( answer.length == 0 )
	{
		var output = "Please enter a valid security answer";
		document.getElementById("answerError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("answerError").textContent = "";
		return true;
	}
}

function ValidateBirthDate( date )
{
	var pickedDate =  new Date( date );
	var todayDate =  new Date();
	
	if( pickedDate > todayDate || pickedDate == "Invalid Date" )
	{
		var output = "Please enter a valid birth date";
		document.getElementById("dateOfBirthError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("dateOfBirthError").textContent = "";
		return true;
	}
}

function ValidateGender( gender )
{
	if( gender == 3 )
	{
		var output = "Please select a gender";
		document.getElementById("genderError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("genderError").textContent = "";
		return true;
	}
}


function ValidateSideAffected( sideAffected )
{
	if( sideAffected == 4 )
	{
		var output = "Please enter an affected side";
		document.getElementById("sideAffectedError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("sideAffectedError").textContent = "";
		return true;
	}
}

function ValidateSeverity( severity )
{
	if( severity == 4 )
	{
		var output = "Please enter a severity";
		document.getElementById("severityError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("severityError").textContent = "";
		return true;
	}
}

function ValidateBilateral( bilateral )
{
	if( bilateral == 2 )
	{
		var output = "Please enter yes or no for bilateral";
		document.getElementById("bilateralError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("bilateralError").textContent = "";
		return true;
	}
}


function ValidateDoa( date )
{
	var pickedDate =  new Date( date );
	var todayDate =  new Date();
	
	if( pickedDate > todayDate || pickedDate == "Invalid Date" )
	{
		var output = "Please enter a affliction date";
		document.getElementById("dateOfAfflctionError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("dateOfAfflctionError").textContent = "";
		return true;
	}
}


function ValidateArmLength( armLength )
{
	if( armLength <= 0 )
	{
		var output = "Please enter a valid arm length";
		document.getElementById("armLengthError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("armLengthError").textContent = "";
		return true;
	}
}


function ValidateLeftNeglect( leftNeglect )
{
	if( leftNeglect == 2 )
	{
		var output = "Please enter yes or no for left neglect";
		document.getElementById("leftNeglectError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("leftNeglectError").textContent = "";
		return true;
	}
}



function ValidateRole( roleSelection )
{
	if( roleSelection == 7 )
	{
		var output = "Please enter a role selection";
		document.getElementById("roleSelectionError").textContent = output;
		return false;
	}
	else
	{
		document.getElementById("roleSelectionError").textContent = "";
		return true;
	}
}


//A function just to validate affliction entries
function ValidateAffliction( form )
{
	// If all validations return true, submit. Else, Alert
	//An if statement interferes with passing output
	var valid = 0; //Store the true/false sum of the following functions. Should equal 8. Change if new fields are added.
	var numberOfFunctions = 6;
	valid += ValidateSideAffected( form.sideAffected.value );
	valid += ValidateSeverity( form.severity.value );
	valid += ValidateBilateral( form.bilateral.value );
	valid += ValidateDoa( form.dateOfAfflction.value );
	valid += ValidateArmLength( form.armLength.value );
	valid += ValidateLeftNeglect( form.leftNeglect.value );
	
	if( valid == numberOfFunctions )
	{
		return 1;
	}
	else
	{
		return 0;
	}
}


function CheckValidForm( form )
{
	//var output = { string: "" }; // String for containing entire error warning for use in alert
	
	// If all validations return true, submit. Else, Alert
	//An if statement interferes with passing output
	var valid = 0; //Store the true/false sum of the following functions. Should equal 8. Change if new fields are added.
	var numberOfFunctions = 10;

	valid += ValidateUsername( form.userName.value );
	valid += ValidatePassword( form.password.value );
	valid += ValidateConfirmPassword( form.password.value, form.confPassword.value );
	valid += ValidateEmail( form.Email.value );
	valid += ValidateFullName( form.fullName.value );
	valid += ValidateQuestion( form.question.value );
	valid += ValidateAnswer( form.answer.value );
	valid += ValidateBirthDate( form.dateOfBirth.value );
	valid += ValidateGender( form.gender.value );
	valid += ValidateRole( form.roleSelection.value );
	
	if( form.roleSelection.value == 5 )
	{
		numberOfFunctions += 1;
		valid += ValidateAffliction( form ); //Returns either 1 or 0
	}

	if( valid == numberOfFunctions )
	{
		form.submit();
	}
	else
	{
		alert( "There is invalid input" );
	}
	
}


function CheckText(id)
{
	var element = document.getElementById(id);
	var i = 0;
	var str = element.value;
	
	
	str = str.replace(/[^a-zA-Z1-9.@\-/,#_ ]/g, "");
	
	//Update real value
	element.value = str;
	
	return true;
	
}
