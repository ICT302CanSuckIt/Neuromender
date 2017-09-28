<?php
	if ($_SESSION['loggedIn'] == false)
	{
		header("Location: Login.php");
		exit();
	}
	error_reporting(0);
	
	
	echo("
			<h1 class='main-title'>Alerts</h1><br/>
		");
	
	if(isset($_POST["btnClear"]))
	{
		$AlertClearSQL = "update Alerts set Seen=1 where ParentID=" . $_SESSION['UserID'] . ";";
		$AlertClearResult = mysqli_query($dbhandle,$AlertClearSQL);
	}
	else if(isset($_POST["btnRecover"]))
	{
		$AlertClearSQL = "update Alerts set Seen=0 where ParentID=" . $_SESSION['UserID'] . " and Description<>'Hidden - Alerts accessed';";
		$AlertClearResult = mysqli_query($dbhandle,$AlertClearSQL);
	}
	
	
	include "GenerateAlerts.php";
	
	
	
	$sql = "";
	
	if( isset( $_GET['user'] ) && isset( $_GET['date'] ) )
	{
		$sql = "select *, DATE_FORMAT(CAST(`Date` AS DATE), '%d-%m-%Y %r') AS DDate from Alerts where Seen=0 and SubjectID=".$_GET['user']." and CAST(`Date` AS DATE)='".$_GET['date']."';";
	}
	else if( isset( $_GET['user'] ) )
	{
		$sql = "select *, DATE_FORMAT(CAST(`Date` AS DATE), '%d-%m-%Y %r') AS DDate from Alerts where Seen=0 AND SubjectID=".$_GET['user'].";";
	}
	else
	{
		$sql = "select * from Alerts where Seen=0 and SubjectID=".$_SESSION['UserID'].";";
	}
	
	
	$result = $dbhandle->query($sql);
	
	if ($result  === FALSE) //Error check
	{
		echo "<br>Error: " . $sql . "<br>" . $dbhandle->error;
	}
	else
	{
		$outputString = "";
		
		$outputString .= "	<form id='alertsForm' method='post' onsubmit='return CheckValidForm(this);'>
							<table class='table'>
								<tr>
									<th class='alertsdata'>
										Name
									</th>
									<th class='alertsdata'>
										Date
									</th>
									<th class='alertsdata'>
										Message
									</th>
								</tr>";
		
		if($result->num_rows == 0 )
			$outputString .= "<tr><td></td><td><p class='alertsrecord'>No Alerts Recorded</p></td><td></td></tr>";
			//$outputString .= "<tr><td></td><td><p class='alertsrecord'>No Alerts Recorded</p></td><td></td></tr>";
			
			
		while( $row = $result->fetch_assoc() )
		{
			$date 			= $row["DDate"];
			$description 	= $row["Description"];
			$patientID	 	= $row["SubjectID"];
			$patientSql = "select FullName, UserID from Users where UserID=$patientID;";
			$patientResult = $dbhandle->query($patientSql);
			$patient = $patientResult->fetch_assoc();
			$patientName = $patient["FullName"];
			$patientID = $patient["UserID"];
			
			$outputString .=	"<tr>
									<td style='text-align:left;padding-left:50px;padding-bottom:10px;'>
										<a href='../Main/Profile.php?user=$patientID&password=2'>$patientName</a>
									</td>
									<td style='text-align:left;padding-left:50px;padding-bottom:10px;'>
										$date
									</td>
									<td style='text-align:left;padding-left:50px;padding-bottom:10px;'>
										$description
									</td>
								</tr>";

		
		}
		$outputString .=	"
		
								<tr class='button-alerts'>
									<td >
										<input class='button-alerts' type='submit' name='btnClear' value='Clear All Alerts'>
									</td>
									<td >
										<input class='button-alerts' type='submit' name='btnRecover' value='Recover All Alerts'>
									</td>
								</tr>
							</table>
						</form>";
		
		echo $outputString;
		
	}
?>
