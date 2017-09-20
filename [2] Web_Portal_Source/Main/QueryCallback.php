<?php 
	include ("../Includes/DBConnect.php"); 
	$outputString = "";
	if ($_SESSION['loggedIn'] == true) {
		if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio || $_SESSION['SelectedRole'] == $constResearch)
		{
			if (!empty($_GET["tableName"])){
				$type = "";
				if (!empty($_GET["type"]))
				{
					$type = $_GET["type"];
				}
				$table = $_GET["tableName"];

				$sqlColumNames = "SHOW COLUMNS FROM $table";

				$result = $dbhandle->query($sqlColumNames);
				
				$liItems = "";
				if ($result->num_rows > 0) 
				{
					while ($row = $result->fetch_assoc()) 
					{
						foreach($row as $key => $value) 
						{
							if($key == "Field")
							{
								if ($type == "list")
								{
                                                                        $description = GetDescription($value);
									$liItems = $liItems . "<li class='ui-state-default' value='$value' data-toggle='tooltip' title='$description'>$value</li>";
								}
								else if ($type == "option")
								{
									$liItems = $liItems . "<option value='$value'>$value</option>";
								}
								//echo "$key = $value <br />";
							}
						}
					}
					$outputString = $liItems;
				}
					
			}
			
		}
		
	} else 
	{
		$outputString = $outputString . "<p>Not Logged In</p></div></div>"; 
	}
	echo $outputString;

?>