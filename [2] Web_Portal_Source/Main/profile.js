var editMode = false;

function SetEdit()
{
	editMode = !editMode;
	var form = document.getElementById("detailForm");
	ShowWingman( form );
	ShowTargets( form );
	//if( form.EnabledWingman.checked == true )
	//{
		//$(".WingmanEdit").toggle();
	//}
}


function ShowCycling( form )
{
	if( form.EnabledCycling.checked == true )
	{
		$(".CyclingData").show();
		
		if( editMode == true )
		{
			$(".CyclingEdit").show();
			$(".CyclingNotEdit").hide();
		}
		else
		{
			$(".CyclingEdit").hide();
			$(".CyclingNotEdit").show();
		}
	}
	else
	{
		$(".CyclingEdit").hide();
		$(".CyclingNotEdit").hide();
		$(".CyclingData").hide();
	}
}


function ShowWingman( form )
{
	if( form.EnabledWingman.checked == true )
	{
		$(".WingmanData").show();
		
		if( editMode == true )
		{
			$(".WingmanEdit").show();
			$(".WingmanNotEdit").hide();
		}
		else
		{
			$(".WingmanEdit").hide();
			$(".WingmanNotEdit").show();
		}
	}
	else
	{
		$(".WingmanEdit").hide();
		$(".WingmanNotEdit").hide();
		$(".WingmanData").hide();
	}
}

function ShowTargets( form )
{
	if( form.EnabledTargets.checked == true )
	{
		$(".TargetData").show();
		
		if( editMode == true )
		{
			$(".TargetEdit").show();
			$(".TargetNotEdit").hide();
		}
		else
		{
			$(".TargetEdit").hide();
			$(".TargetNotEdit").show();
		}
	}
	else
	{
		$(".TargetEdit").hide();
		$(".TargetNotEdit").hide();
		$(".TargetData").hide();
	}
}


//--------Validate Affliction------------------//
function ValidateNotes( notes, output )
{
	if( notes.length == 0 )
	{
		//output.string += "Please enter valid notes\n";
		return false;
	}
	else
	{
		return true;
	}
}


//--------Validate Wingman Restrictions--------//
function ValidateAngleThreshold( threshold, output )
{
	if( threshold <= 0 || isNaN(threshold) || threshold=="" )
	{
		//output.string += "Please enter valid angled threshold\n";
		document.getElementById("angleThresholdError").textContent = "Please enter valid angle threshold";
		return false;
	}
	else
	{
		document.getElementById("angleThresholdError").textContent = "";
		return true;
	}
}


function ValidateArmMaxExtension( threshold, output )
{
	if( threshold <= 0 || isNaN(threshold) || threshold=="" )
	{
		//output.string += "Please enter valid angled threshold\n";
		document.getElementById("ArmMaxExtensionError").textContent = "Please enter valid maximum arm extension";
		return false;
	}
	else
	{
		document.getElementById("ArmMaxExtensionError").textContent = "";
		return true;
	}
}

function ValidateAngleThresholdIncrease( increase, output )
{
	if( increase < 0 || isNaN(increase) || increase=="" )
	{
		//output.string += "Please enter a valid angle threshold increase\n";
		document.getElementById("thresholdIncreaserError").textContent = "Please enter a valid angle threshold increase";
		return false;
	}
	else
	{
		document.getElementById("thresholdIncreaserError").textContent = "";
		return true;
	}
}

function ValidateSlowTrack( speed, output )
{
	if( speed <= 0 || isNaN(speed) || speed=="" )
	{
		//output.string += "Please enter a valid slow track speed\n";
		document.getElementById("speedSlowError").textContent = "Please enter a valid slow track speed";
		return false;
	}
	else
	{
		document.getElementById("speedSlowError").textContent = "";
		return true;
	}
}

function ValidateDistanceShort( speed, output )
{
	if( speed <= 0 || isNaN(speed) || speed=="" )
	{
		//output.string += "Please enter a valid slow track speed\n";
		document.getElementById("DistanceShortError").textContent = "Please enter a valid slow track speed";
		return false;
	}
	else
	{
		document.getElementById("DistanceShortError").textContent = "";
		return true;
	}
}


function ValidateDistanceMedium( speed, output )
{
	if( speed <= 0 || isNaN(speed) || speed=="" )
	{
		//output.string += "Please enter a valid slow track speed\n";
		document.getElementById("DistanceMediumError").textContent = "Please enter a valid slow track speed";
		return false;
	}
	else
	{
		document.getElementById("DistanceMediumError").textContent = "";
		return true;
	}
}

function ValidateDistanceLong( speed, output )
{
	if( speed <= 0 || isNaN(speed) || speed=="" )
	{
		//output.string += "Please enter a valid slow track speed\n";
		document.getElementById("DistanceLongError").textContent = "Please enter a valid slow track speed";
		return false;
	}
	else
	{
		document.getElementById("DistanceLongError").textContent = "";
		return true;
	}
}



function ValidateMediumTrack( speed, output )
{
	if( speed <= 0 || isNaN(speed) || speed=="" )
	{
		//output.string += "Please enter a valid medium track speed\n";
		document.getElementById("speedMediumError").textContent = "Please enter a valid medium track speed";
		return false;
	}
	else
	{
		document.getElementById("speedMediumError").textContent = "";
		return true;
	}
}

function ValidateFastTrack( speed, output )
{
	if( speed <= 0 || isNaN(speed) || speed=="" )
	{
		//output.string += "Please enter a valid fast track speed\n";
		document.getElementById("speedFastError").textContent = "Please enter a valid fast track speed";
		return false;
	}
	else
	{
		document.getElementById("speedFastError").textContent = "";
		return true;
	}
}

function ValidateWingGamesPerDay( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of wingman games per day\n";
		document.getElementById("WGamesPerDayError").textContent = "Please enter a valid number of wingman games per day";
		return false;
	}
	else
	{
		document.getElementById("WGamesPerDayError").textContent = "";
		return true;
	}
}

function ValidateWingGamesPerSession( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of wingman games per session\n";
		document.getElementById("WGamesPerSessionError").textContent = "Please enter a valid number of wingman games per session";
		return false;
	}
	else
	{
		document.getElementById("WGamesPerSessionError").textContent = "";
		return true;
	}
}

function ValidateWingInterval( num, output )
{
	if( num < 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid interval between wingman sessions\n";
		document.getElementById("WIntervalBetweenSessionError").textContent = "Please enter a valid interval between wingman sessions";
		return false;
	}
	else
	{
		document.getElementById("WIntervalBetweenSessionError").textContent = "";
		return true;
	}
}


//----- VALIDATE CYCLING RESTRICTIONS --//

function ValidateCycleGamesPerDay( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of cycling games per day\n";
		document.getElementById("CGamesPerDayError").textContent = "Please enter a valid number of cycling games per day";
		return false;
	}
	else
	{
		document.getElementById("CGamesPerDayError").textContent = "";
		return true;
	}
}

function ValidateCycleGamesPerSession( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of cycling games per session\n";
		document.getElementById("CGamesPerSessionError").textContent = "Please enter a valid number of cycling games per session";
		return false;
	}
	else
	{
		document.getElementById("CGamesPerSessionError").textContent = "";
		return true;
	}
}

function ValidateCycleInterval( num, output )
{
	if( num < 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid interval between cycling sessions\n";
		document.getElementById("CIntervalBetweenSessionError").textContent = "Please enter a valid interval between cycling sessions";
		return false;
	}
	else
	{
		document.getElementById("CIntervalBetweenSessionError").textContent = "";
		return true;
	}
}



function ValidateSensorDistance ( num, output )
{
	if( num < 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid interval between cycling sessions\n";
		document.getElementById("SensorDistanceError").textContent = "Please enter a valid distance for the sensor";
		return false;
	}
	else
	{
		document.getElementById("SensorDistanceError").textContent = "";
		return true;
	}
}





//--------------------//










//--------Validate Target Restrictions---------//
function ValidateGridSize( row, col, output )
{
	if( row <= 0 || col <= 0 || isNaN(row) || isNaN(col) || row=="" || col=="" )
	{
		//output.string += "Please enter a valid row and column size for the grid\n";
		document.getElementById("gridSizeError").textContent = "Please enter valid rows/columns";
		return false;
	}
	else
	{
		document.getElementById("gridSizeError").textContent = "";
		return true;
	}
}

function ValidateGridOrder( order, output )
{
	var i = 0;
	var orderStr = order.value;
	
	for( i = orderStr.length-1; i >= 0; i-- )
	{
		if(isNaN(orderStr[i]) && orderStr[i] != ',')
		{
			orderStr = orderStr.slice(0,i) + orderStr.slice(i+1);
		}
	}
	
	//Get rid of spaces and un-needed commas
	orderStr = orderStr.replace(/^[,\s]+|[,\s]+$/g, '').replace(/,[,\s]*,/g, ',');
	
	
	//Check the values are within range
	var orderArray = orderStr.split( "," ); 	//Create array of numbers
	var changed = false;
	
	for( i = orderArray.length-1; i >= 0; i-- )
	{
		if(orderArray[i] > document.getElementById("gridSizeRow").value * document.getElementById("gridSizeCol").value)
		{
			orderArray.splice( i, 1 );				//Remove the item at orderNum
			changed = true;
		}
	}
	
	if(changed)
	{
		//If the grid order contains anything, refill the string with the updated contents
		if(orderArray.length > 0)
		{
			orderStr = String(orderArray[0]);		//Do this just so we don't put a comma at the beginning
			
			for( i = 1; i < orderArray.length; i++ )	//For each order entry
			{
				orderStr += ","+orderArray[i];		//Append the gridOrderStr with the appropriate order item
			}
		}
	}
	
	
	if( orderStr.length == 0 )
	{
		document.getElementById("gridOrderError").textContent = "Please enter a target order";
		return false
	}
	else
	{
		//Update real value
		order.value = orderStr;
		return true;
	}
	
	
	
}

function ValidateRepetitions( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of target repetitions\n";
		document.getElementById("repetitionsError").textContent = "Please enter a valid number of target repetitions";
		return false;
	}
	else
	{
		document.getElementById("repetitionsError").textContent = "";
		return true;
	}
}

function ValidateExtensionThreshold( threshold, output )
{
	var armLength = document.getElementById("ArmLength").value;
	var EnabledTargets = document.getElementById("EnabledTargets").checked;
	var valid = true;
	
	document.getElementById("extensionThresholdError").textContent = "";
	
	if( threshold > armLength && EnabledTargets == true )
	{
		document.getElementById("extensionThresholdError").textContent += "Extension Threshold cannot be greater than arm length\n";
		valid = false;
	}
	
	if( threshold <= 0 || isNaN(threshold) || threshold=="" )
	{
		//output.string += "Please enter a valid extension threshold\n";
		document.getElementById("extensionThresholdError").textContent += "Please enter a valid extension threshold";
		return false;
	}
	
	
	return valid;
}

function ValidateTargetMinimumThreshold( threshold, output )
{
	if( threshold <= 0 || isNaN(threshold) || threshold=="" )
	{
		//output.string += "Please enter a valid minimum extension threshold\n";
		document.getElementById("minimumExtensionThresholdError").textContent = "Please enter a valid minimum extension threshold";
		return false;
	}
	else
	{
		document.getElementById("minimumExtensionThresholdError").textContent = "";
		return true;
	}
}

function ValidateExtensionIncrease( increase, output )
{
	if( increase < 0 || isNaN(increase) || increase=="" )
	{
		//output.string += "Please enter a valid extension threshold increment/decrement\n";
		document.getElementById("extensionThresholdIncreaseError").textContent = "Please enter a valid extension threshold increment/decrement";
		return false;
	}
	else
	{
		document.getElementById("extensionThresholdIncreaseError").textContent = "";
		return true;
	}
}

function ValidateTargetGamesPerDay( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of targets games per day\n";
		document.getElementById("TGamesPerDayError").textContent = "Please enter a valid number of targets games per day";
		return false;
	}
	else
	{
		document.getElementById("TGamesPerDayError").textContent = "";
		return true;
	}
}

function ValidateTargetGamesPerSession( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid number of targets games per session\n";
		document.getElementById("TGamesPerSessionError").textContent = "Please enter a valid number of targets games per session";
		return false;
	}
	else
	{
		document.getElementById("TGamesPerSessionError").textContent = "";
		return true;
	}
}

function ValidateTargetInterval( num, output )
{
	if( num < 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid interval between targets sessions\n";
		document.getElementById("TIntervalBetweenSessionError").textContent = "Please enter a valid interval between targets sessions";
		return false;
	}
	else
	{
		document.getElementById("TIntervalBetweenSessionError").textContent = "";
		return true;
	}
}

function ValidateCountdownDistance( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid countdown distance\n";
		document.getElementById("CountdownDistanceError").textContent = "Please enter a valid countdown distance";
		return false;
	}
	else
	{
		document.getElementById("CountdownDistanceError").textContent = "";
		return true;
	}
}

function ValidateAdjustmentCountdown( num, output )
{
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid adjustment countdown\n";
		document.getElementById("AdjustmentCountdownError").textContent = "Please enter a valid adjustment countdown";
		return false;
	}
	else
	{
		document.getElementById("AdjustmentCountdownError").textContent = "";
		return true;
	}
}



//----Validate affliction----//
function ValidateDoa( doa, output )
{
	var pickedDate =  new Date( doa );
	var todayDate =  new Date();
	
	if( pickedDate > todayDate || pickedDate == "Invalid Date"  )
	{
		//output.string += "Please enter a valid affliction date\n";
		document.getElementById("DateOfAfflictionError").textContent = "Please enter a valid affliction date";
		return false;
	}
	else
	{
		document.getElementById("DateOfAfflictionError").textContent = "";
		return true;
	}
}

function ValidateArmLength( num, output )
{
	ValidateExtensionThreshold( document.getElementById("extensionThreshold").value );
	if( num <= 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid arm length\n";
		document.getElementById("ArmLengthError").textContent = "Please enter the valid arm length";
		return false;
	}
	else
	{
		document.getElementById("ArmLengthError").textContent = "";
		return true;
	}
}





//Validate User details----//
function ValidateFullName( name, output )
{
	if( name.length == 0 )
	{
		//output.string += "Please enter a valid name\n";
		document.getElementById("fNameError").textContent = "Please enter a valid name";
		return false;
	}
	else
	{
		document.getElementById("fNameError").textContent = "";
		return true;
	}
}

function ValidateUsername( username, output )
{
	if( username.length == 0 )
	{
		//output.string += "Please enter a valid username\n";
		document.getElementById("uNameError").textContent = "Please enter a valid username";
		return false;
	}
	else
	{
		document.getElementById("uNameError").textContent = "";
		return true;
	}
}

function ValidateEmail( email, output )
{
	var regex = /\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b/;
	if( email.length == 0 || !regex.test(email) )
	{
		//output.string += "Please enter a valid email\n";
		document.getElementById("emailError").textContent = "Please enter a valid email";
		return false;
	}
	else
	{
		document.getElementById("emailError").textContent = "";
		return true;
	}
}

function ValidateBirthDate( date, output )
{
	var pickedDate =  new Date( date );
	var todayDate =  new Date();
	
	if( pickedDate > todayDate || pickedDate == "Invalid Date"  )
	{
		//output.string += "Please enter a valid birth date\n";
		document.getElementById("dobError").textContent = "Please enter a valid birth date";
		return false;
	}
	else
	{
		document.getElementById("dobError").textContent = "";
		return true;
	}
}


function ValidateArmResetDistance( num, output )
{
	if( num < 0 || isNaN(num) || num=="" )
	{
		//output.string += "Please enter a valid arm length\n";
		document.getElementById("ArmResetDistanceError").textContent = "Please enter a valid arm reset distance";
		return false;
	}
	else
	{
		document.getElementById("ArmResetDistanceError").textContent = "";
		return true;
	}
}



function CheckValidForm(window)
{
	var form = document.getElementById("detailForm");
	var output = { string: "" }; // String for containing entire error warning for use in alert
	
	// If all validations return true, submit. Else, Alert
	//An if statement interferes with passing output
	var valid = 0; //Store the true/false sum of the following functions.
	var numberOfFunctions = 6;

	//Validate user details
	valid += ValidateFullName( form.fName.value, output );
	valid += ValidateUsername( form.uName.value, output );
	valid += ValidateEmail( form.email.value, output );
	valid += ValidateBirthDate( form.dob.value, output );
	
	//Validate affliction
	valid += ValidateDoa( form.DateOfAffliction.value, output );
	valid += ValidateArmLength( form.ArmLength.value, output );
	//valid += ValidateNotes( form.Notes.value, output );
	
	if( form.EnabledWingman.checked == true )
	{
		numberOfFunctions += 8;
	
		valid += ValidateAngleThreshold( form.angleThreshold.value, output );
		valid += ValidateAngleThresholdIncrease( form.thresholdIncreaser.value, output );
		valid += ValidateSlowTrack( form.speedSlow.value, output );
		valid += ValidateMediumTrack( form.speedMedium.value, output );
		valid += ValidateFastTrack( form.speedFast.value, output );
		valid += ValidateWingGamesPerDay( form.WGamesPerDay.value, output );
		valid += ValidateWingGamesPerSession( form.WGamesPerSession.value, output );
		valid += ValidateWingInterval( form.WIntervalBetweenSession.value, output );
	}
	
	if( form.EnabledTargets.checked == true )
	{
		numberOfFunctions += 12;
	
		valid += ValidateGridSize( form.gridSizeRow.value, form.gridSizeCol.value, output );
		valid += ValidateGridOrder( form.gridOrder, output );
		valid += ValidateRepetitions( form.repetitions.value, output );
		valid += ValidateExtensionThreshold( form.extensionThreshold.value, output );
		valid += ValidateTargetMinimumThreshold( form.minimumExtensionThreshold.value, output );
		valid += ValidateExtensionIncrease( form.extensionThresholdIncrease.value, output );
		valid += ValidateTargetGamesPerDay( form.TGamesPerDay.value, output );
		valid += ValidateTargetGamesPerSession( form.TGamesPerSession.value, output );
		valid += ValidateTargetInterval( form.TIntervalBetweenSession.value, output );
		valid += ValidateCountdownDistance( form.CountdownDistance.value, output );
		valid += ValidateAdjustmentCountdown( form.AdjustmentCountdown.value, output );
		valid += ValidateArmResetDistance( form.ArmResetDistance.value, output );
	}
	
	if( valid == numberOfFunctions )
	{
		return true;
		//form.submit();
	}
	else
	{
		alert( "There is invalid input" );
		return false;
	}
	
	return false;
}




function SizeChanged()
{
	document.getElementById("gridOrder").value = "";
	RegenerateTable();
}

function GridOrderChanged()
{
	var temp;
	ValidateGridOrder(document.getElementById("gridOrder"), temp);
	RegenerateTable();
}

function CheckText(id)
{
	var element = document.getElementById(id);
	var i = 0;
	var str = element.value;
	
	
	str = str.replace(/[^a-zA-Z1-9.@ ]/g, "");
	
	//Update real value
	element.value = str;
	
	return true;
	
}
