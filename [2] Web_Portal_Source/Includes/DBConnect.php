<?php
ini_set('session.cookie_lifetime', 60 * 60);
ini_set('max_execution_time', 0);
ini_set('memory_limit', '-1');

session_start();
error_reporting(E_ALL); 
ini_set('display_errors', 1);


/*$username = "neuroadmin4";
//change the following to the ones you are using
$password = "*7BA8FB152973F6C4434C6BC54E94A7366969EDFE";
//$hostname = "murdoch.edu.au";
$hostname = "127.0.0.1";
$dbname = "neuromender";*/
//replacing database details with external file -- does not currently work
//Declare name of Data File
//open file
$filename = __DIR__ . "./DBData.txt";
$DBFile = fopen($filename, "r") or die ("Unable to Open File.");
//get database details from file
$username = rtrim(fgets($DBFile));
$password = rtrim(fgets($DBFile));
$hostname = rtrim(fgets($DBFile));
$dbname = rtrim(fgets($DBFile));

// the role types
$constSuperAdmin = 0;
$constAdmin = 1;
$constCoach = 2;
$constPhysio = 3;
$constResearch = 4;
$constPatient = 5;

$chartIncrement = 0;

//connection to the database
$dbhandle = mysqli_connect($hostname, $username, $password, $dbname);
  if (!$dbhandle) {
	  echo "Failed to connect to MySQL: (" . mysqli_connect_errno() . ") " . mysqli_connect_error();
		exit;
  }
  
function getUserRoles($id, $dbhandle)
{
	$sql = " Select Role.Description
			from AssignedRoles
			LEFT JOIN Role on Role.RoleID = AssignedRoles.RoleID
			Where UserID = $id";
			
		//var_dump($sql);
			
	$result = $dbhandle->query($sql);
	//var_dump($result);
	
	$roles = "";
	if ($result->num_rows > 0) 
	{
		while($row = $result->fetch_assoc()) 
		{
			if($roles == "")
			{
				$roles .= $row["Description"];
			}
			else
			{
				$roles .= ",".$row["Description"];
			}
		}
	}
	return $roles;
}
  

//This function takes in an SQL query and returns a select box from it.
//$sql <-- The query to select the data from the database.
//$selectBoxID <-- The uniqueID of the select box being created
//$selectBoxName <-- The Name of the select box being created
//$IDField <-- The Name of the Field in the database that will be the ID example - LesionID
//$IDValue <-- The Name of the Description to be displayed example - Description
//$onChangeFunc <-- if there is a javascript onchange function call put it here.
function CreateSelectBox($sql, $selectBoxID, $selectBoxName, $IDField, $IDValue, $onChangeFunc, $dbhandle)
{
        $prevVal = -1; 
	$result = $dbhandle->query($sql);
	$OutputStr = "<select name='$selectBoxName' id='$selectBoxID' onchange='$onChangeFunc'>";
	
	if ($result->num_rows > 0) {
		// output data of each row
		while($row = $result->fetch_assoc()) 
                {
                        if ($row[$IDField] != $prevVal)
                        {
                            $OutputStr = $OutputStr . "<option value='".$row[$IDField]."'>".$row[$IDValue]."</option>";
                            $prevVal = $row[$IDField];
                        }
		}
	}
	else
	{
		$OutputStr = $OutputStr . "<option value=''>NoneFound</option>";
	}
	$OutputStr = $OutputStr . "</select>";
	return $OutputStr;
}

//same as above with selection persistence
function CreatePersistenceSelectBox($sql, $selectBoxID, $selectBoxName, $IDField, $IDValue, $selectedField, $onChangeFunc, $dbhandle)
{
    $prevVal = -1; 
	$result = $dbhandle->query($sql);
	$OutputStr = "<select name='$selectBoxName' id='$selectBoxID' onchange='$onChangeFunc'>";
	
	if ($result->num_rows > 0) {
		// output data of each row
		while($row = $result->fetch_assoc()) {
            if ($row[$IDField] != $prevVal) {
                $field = $row[$IDField];
                $selected = "";
                if($field == $selectedField){
                    $selected = "selected";
                }
                $OutputStr = $OutputStr . "<option value='".$row[$IDField]."' ".$selected.">".$row[$IDValue]."</option>";
                $prevVal = $row[$IDField];
            }
		}
	}
	else {
		$OutputStr = $OutputStr . "<option value=''>NoneFound</option>";
	}
	$OutputStr = $OutputStr . "</select>";
	return $OutputStr;
}

function CreateDownloadSelectBox($sql, $selectBoxID, $selectBoxName, $IDField, $IDValue, $onChangeFunc, $dbhandle)
{
        $description;
	$result = $dbhandle->query($sql);
	$OutputStr = "<select name='$selectBoxName' id='$selectBoxID' onchange='$onChangeFunc'>";
	
	if ($result->num_rows > 0) {
		// output data of each row
		while($row = $result->fetch_assoc()) 
                {
                            $description = $row[$IDValue];
                            $OutputStr = $OutputStr . "<option value='".$row[$IDField]."'data-toggle='tooltip' title='$description'>".$row[$IDValue]."</option>";
		}
	}
	else
	{
		$OutputStr = $OutputStr . "<option value=''>NoneFound</option>";
	}
	$OutputStr = $OutputStr . "</select>";
	return $OutputStr;
}

function getval($dbhandle, $sql) {
    $result = $dbhandle->query($sql);
    $value = $result->fetch_array(MYSQLI_NUM);
    return is_array($value) ? $value[0] : "";

}

function numToDetail($num, $type)
{
	$detail = "";
	if($type == "gender")
	{
		if($num == "0")
		{
			$detail = "Male";
		}
		else if($num == "1")
		{	
			$detail = "Female";
		}
	}
	else if($type == "yesNo")
	{
		if($num == "0")
		{
			$detail = "No";
		}
		else if($num == "1")
		{	
			$detail = "Yes";
		}
	}
	return $detail;
}

function hasViewingRights($Child, $dbhandle)
{
	$LoggedInUser = $_SESSION['UserID'];
	$currentRole = (int)$_SESSION['SelectedRole'];
	
	$AdditionalDepth = "";
	if ($currentRole == 1 || $currentRole == 0)// if the user looking is a superadmin
		return true;
	else{
		$sql = "select t.`UserID`, t.`FullName`, @pv := t.`ParentID` col3
				from (select * from Users order by `UserID` desc) t
				join (select @pv := $Child) tmp
				where t.`UserID` = @pv";
		$result = $dbhandle->query($sql);
		if ($result->num_rows > 0) {
			// output data of each row
			while($row = $result->fetch_assoc()) {
				if((int)$row['UserID'] != $Child)
				{
					if((int)$row['UserID'] == $LoggedInUser)
					{
						return true;
					}
				}
			}
		}
		return false;
	}
}

function GetDescription($value)
{
    $output; 
    
    switch ($value)
    {
        // Achievement Table Descriptions
        case "AcheivementID":
            $output = "Unique identifier of the Achievement";
            break;
        case "SessionID":
            $output = "Session that has been assigned to this table";
            break;
        case "TaskID":
            $output = "Task that has been assigned to this table";
            break;
        case "ThresholdPassed":
            $output = "Threshold (either in angles or cm) that was passed.";
            break;
        case "TimeAchieved":
            $output = "The date and time that this was made";
            break;
        
        // Raw Tracking Table Descriptions
        case "RawTrackingID":
            $output = "Unique identifier of the tracking information per session";
            break;
        case "AverageBodyDepth":
            $output = "Average distance of the user from the Kinect";
            break;
        case "CentralPontX":
            $output = "Unity’s related X coord for Centre of Characters chest";
            break;
        case "CentralPontY":
            $output = "Unity’s related Y coord for Centre of Characters chest";
            break;
        case "CentralPontZ":
            $output = "Unity’s related Z coord for Centre of Characters chest";
            break;
        case "RightHandX":
            $output = "Unity’s related X coord for Characters Right Hand";
            break;
        case "RightHandY":
            $output = "Unity’s related Y coord for Characters Right Hand";
            break;
        case "RightHandZ":
            $output = "Unity’s related Z coord for Characters Right Hand";
            break;
        case "LeftHandX":
            $output = "Unity’s related X coord for Characters Left Hand";
            break;
        case "LeftHandY":
            $output = "Unity’s related Y coord for Characters Left Hand.";
            break;
        case "LeftHandZ":
            $output = "Unity’s related Z coord for Characters Left Hand";
            break;
        case "RightElbowX":
            $output = "Unity’s related X coord for Characters Right Elbow";
            break;
        case "RightElbowY":
            $output = "Unity’s related Y coord for Characters Right Elbow";
            break;
        case "RightElbowZ":
            $output = "Unity’s related Z coord for Characters Right Elbow";
            break;
        case "LeftElbowX":
            $output = "Unity’s related X coord for Characters Left Elbow";
            break;
        case "LeftElbowY":
            $output = "Unity’s related Y coord for Characters Left Elbow";
            break;
        case "LeftElbowZ":
            $output = "Unity’s related Z coord for Characters Left Elbow";
            break;
        case "LeftAngle":
            $output = "Angle in degrees from a line going down the centre of the body and another from the shoulder to the left elbow";
            break;
        case "RightAngle":
            $output = "Angle in degrees from a line going down the centre of the body and another from the shoulder to the right elbow";
            break;
        case "Time":
            $output = "Time that this was taken";
            break;
        // Reach Game Data Table Descriptions
        case "UserID":
            $output = "Unique identifer for the user";
            break;
        case "GameNoID":
            $output = "Identifer for the game that was played in a session";
            break;
        case "RoundID":
            $output = "Identifer for the round that was played in a game";
            break;
        case "Accuracy":
            $output = "Accuracy the patient performed at for hitting a target";
            break;
        case "MaximumReach";
            $output = "Maximum extension the patient performed during a round";
            break;
        case "MinimumReach";
            $output = "Minimum extension the patient performed during a round";
            break;
        // Eye Tracking Data Table Descriptions
        case "EyeTrackingID";
            $output = "Unique identifer for the eye tracking moment";
            break;
        case "Time";
            $output = "Time that the eye tracker positions were recorded";
            break;
        case "EyeX";
            $output = "Position of the eyes on the X-axis";
            break;
        case "EyeY";
            $output = "Position of the eyes on the Y-axis";
            break;
        case "ObjectID";
            $output = "Unique identifer for an object";
            break;
        
        // Level Completed Table Descriptions
        case "LevelCompletedID";
            $output = "Unique identifier of the individual level completion";
            break;
        case "LevelID";
            $output = "Unique identifier of the level that was completed";
            break;
        case "SelectedDuration";
            $output = "Selected duration for the level";
            break;
        case "TimeStarted";
            $output = "Time the completed level was started";
            break;
        case "TimeFinished";
            $output = "Time the completed level was finished";
            break;
        // Session Table Descriptions
        case "WingmanPlayed";
            $output = "Identifer to see if wingman game has been played";
            break;
        case "TargetsPlayed";
            $output = "Identifer to see if the target game has been played";
            break;
        case "StartTime";
            $output = "Time that the session was started";
            break;
        case "EndTime";
            $output = "Time that the session was ended";
            break;
        case "TotalScore";
            $output = "Score achieved by the user";
            break;
        case "TaskCompleted";
            $output = "Number of tasks compeleted by the user";
            break;
        
        default:
            $output = "No description available";
    }
    return $output;
}

function GetTableDescription($inTable)
{
    $output;
    $output = $inTable;
    return $output;
}


?>
