<link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
  <script src="//code.jquery.com/jquery-1.10.2.js"></script>
  <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
  <script type='text/javascript' src='QueryBuilder.js'></script>
  <link rel="stylesheet" href="/resources/demos/style.css">
	<style>
	  #fieldList, #Selectedfields {
		border: 1px solid #000000;
		width: 240px;
		min-height: 400px;
		list-style-type: none;
		margin: 0;
		padding: 5px 0 0 0;
		float: left;
		margin-right: 10px;
	  }
	  #fieldList li, #Selectedfields li {
		margin: 0 5px 5px 5px;
		padding: 5px;
		font-size: 1.2em;
		width: 190px;
	  }
  </style>
<?php 
		error_reporting(0);

		$currTable;
		$outputString;
		//$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) {
			if($_SESSION['SelectedRole'] == $constSuperAdmin)
			{
				$selectUsers = $_POST['selectUsers'];
				//$_SESSION['selectUsers'] = $_POST['selectUsers'];
				if (!empty($_POST)){
					if (!empty($_POST["btnRunQuery"]))
					{
						$selectFields = $_POST["selectedValues"];
						$table = $_POST["table"];
						$where = $_POST["where"];
						
						$where1 = $_POST["where1"];
						$where2 = $_POST["where2"];
						$where3 = $_POST["where3"];
						
						$cond_2 = $_POST["cond_2"];
						$cond_3 = $_POST["cond_3"];
						
						$op_1 = $_POST["op_1"];
						$op_2 = $_POST["op_2"];
						$op_3 = $_POST["op_3"];
						
						$val_1 = $_POST["val_1"];
						$val_2 = $_POST["val_2"];
						$val_3 = $_POST["val_3"];
						
						if ($selectFields == "")
							$selectFields = "*";
						
						$sql =  "Select $selectFields from $table";
                                                //$currTable = $table;
								
						if($where == "yes")
						{
							if($op_1 != "0" && $val_1 != "0")
							{
								$sql = $sql . " Where $where1 $op_1 $val_1";
							}
							
							if($cond_2 != "0" && $op_2 != "0" && $val_2 != "0")
							{
								$sql = $sql . " $cond_2 $where2 $op_2 $val_2";
							}
							
							if($cond_3 != "0" && $op_3 != "0" && $val_3 != "0")
							{
								$sql = $sql . " $cond_3 $where3 $op_3 $val_3";
							}
						}
							
						if(strpos($sql, "delete") === false && strpos($sql, "insert") === false )
						{
							//echo "for debugging " .$sql . "<br />";
							$result = $dbhandle->query($sql);
							if ($result->num_rows > 0) 
							{
								var_dump($result);
								
								$date = date("ymd");
								
								$output = fopen("php://output", "w");
								
								$fields = $result->field_count;
								$header = "";
								$body = "";
								$count = 0;
								
								$array = array();
								$headerarray = array();

								

								while ($row = $result->fetch_assoc()) 
								{
									foreach($row as $key => $value) 
									{
										//echo "$key - $value <br/>";
										if($count == 0)
										{
											array_push($headerarray, $key);
										}
										
										array_push($array, $value);
									}
									$body .= "\n";
									 // here you can change delimiter/enclosure
									if($count == 0)
									{
										fputcsv($output, $headerarray);
									}
									$count++;
									fputcsv($output, $array);
								}
								
								fclose($output);
								 

								//echo "$header\n$body";
								
							}
						}
						
					}
				}
				else
				{
					
					$outputString="<h1 class='main-title'> Neuromender Query Builder / CSV Exporter</h1>
						<span class='manuals-content'>Instructions:<br /> </span>
							<ol class='manuals-para2'>
								<li>Select the table you want to get data from.</li>
								<li>Drag and drop any field from the table in 'Available' into 'Selected'.</li>
								<li>If you wish to select everything from the table, leave the 'Selected' field blank</li>
								<li>If you want to add any conditions to the query then set 'Use Where Clause' to 'Yes'.</li>
								<li>Add in any conditions to the fields provided. ie: SessionID = 5 AND UserID = 15</li>
								<li>Click 'Run Query' to get your csv of data from the database.</li>
								(Note that you can only download from one table at a time. If no results would be found, a popup will inform you of the error, then redirect you back to this page.)
							</ol>
							<br/><br/>
						<span class='manuals-content'>Table Legend:<br></span>
							<ol class='manuals-para2'>
								<li>Achievement: Refers to the Wingman game, in regards to overall game data</li>
								<li>RawTracking: Information regarding to data tracked during a session</li>
								<li>Targets Game Data: Refers to the Targets game</li>
								<li>Rowing Game Data: Refers to the Rowing Game</li>
								<li>LevelCompleted: Refers to information on a completed level</li>
								<li>Session: Refers to information about a session</li>
							</ol>
						<br><br>
							<table class='manuals-table'>
								<tr>
									<th>
										Table:
										<form action='./QueryCSV.php' method='post'>
									</th>

									<td>
										<select name='table' id='table' onchange='RepopulateSelectors()'>
											<option value='Achievement'>Achievement</option>
											<option value='ReachGameData'>Targets Game Data</option>
											<option value='CyclingGameData'>Rowing Game Data</option>
											<option value='RawTracking'>RawTracking</option>
											<option value='LevelCompleted'>LevelCompleted</option>
											<option value='Session'>Session</option>
										</select>
									</td>
                </tr>
								<tr>
									<th>
										Fields:
									</th>
									<td style='vertical-align:top;'>
										<h3>Available</h3>
										<ul id='fieldList' class='connectedSortable'>
											%SELECTOPTIONS%											  
										</ul>
									</td>
									<td style='vertical-align:top;'>
										<h3>Selected</h3>
										<ul id='Selectedfields' class='connectedSortable' onAppend='checkSelectedValues()'>
										</ul>
										<input type='hidden' name='selectedValues' id='selectedValues' value='' />
									</td>
								</tr>
								<tr>
									<th>
										Where:
									</th>
									<td>
										Use Where Clause:
										<select name='where' id='where'>
											<option value='no'>no</option>
											<option value='yes'>yes</option>
										</select>
									</td>
									<td style='vertical-align:top;'>
										%WHERE1%
										%WHERE2%
										%WHERE3%
									</td>
								</tr>
								<tr>									
									<td>
										<input type='submit' class='btn btn-primary btn-sm' name='btnRunQuery' value='Run Query' onClick='checkSelectedValues()' />
										</form>
									</td>
								</tr>
							</table>";
									
					$sqlColumNames = "SHOW COLUMNS FROM Achievement";
					
					$result = $dbhandle->query($sqlColumNames);

					$liItems = "";
					$whereItems = "";
					if ($result->num_rows > 0) 
					{
						while ($row = $result->fetch_assoc()) 
						{
							foreach($row as $key => $value) 
							{
								if($key == "Field")
								{
									$description = GetDescription($value);
									$liItems = $liItems . "<li class='ui-state-default' value='$value' data-toggle='tooltip' title='$description'>$value</li>";
									$whereItems = $whereItems . "<option value='$value'>$value</option>";
								}
							}
						}
						$outputString = str_replace("%SELECTOPTIONS%",$liItems, $outputString);
                                                
						$where1 = "<select name='where1' id='where1'>$whereItems</select>".operatorSelect("op_1")."<input type='text' name='val_1' id='val_1' \><br/>" ;
						$outputString = str_replace("%WHERE1%",$where1, $outputString);
						$where2 = conditionSelect("cond_2") . "<select name='where2' id='where2'>$whereItems</select>".operatorSelect("op_2")."<input type='text' name='val_2' id='val_2' \><br/>" ;
						$outputString = str_replace("%WHERE2%",$where2, $outputString);
						$where3 = conditionSelect("cond_3") . "<select name='where3' id='where3'>$whereItems</select>".operatorSelect("op_3")."<input type='text' name='val_3' id='val_3' \><br/>" ;
						$outputString = str_replace("%WHERE3%",$where3, $outputString);
					}
                                        
						// Download All data from selected USERID
						if($_SESSION['SelectedRole'] == $constSuperAdmin)
						{
							$outputString = $outputString . "<br><br><table class='manuals-table'><tr><td>Select a user and download all their data </td>";
							$outputString = $outputString . "<form action='./QueryUserDataCSV.php' method='post'>";
							$outputString = $outputString . "<td>%SELECTUSERS%</td><tr><td></td><td></td></tr>";
							$outputString = $outputString . "<td><input type='submit' class='btn btn-primary btn-sm' name='btnRunQueryFlatFile' value='Download' onClick=''></form></td></table>";
							$outputString = $outputString . "<br><p class='manuals-para'> If there is a lot of data in the tables, you may need to wait a few minutes </p></div>";
							$sql = "SELECT * FROM Users"; // Add in for only their patients
							$outputString = str_replace("%SELECTUSERS%", CreateSelectBox($sql, 'selectUsers', 'selectUsers', 'UserID', 'Username', '', $dbhandle), $outputString);
						}
				}	
			}
			else
			{
				$outputString = $outputString . "<p>You dont have the correct permissions to view this page.</p></div></div>"; 
			}
		} else 
		{
			$outputString = $outputString . "<p>Not Logged In</p></div></div>"; 
		}
		echo $outputString;
		
		function conditionSelect($name)
		{
			return "<select name='$name' id='$name'><option value='0'></option><option value='AND'>AND</option><option value='OR'>OR</option></select>";
		}
		
		function operatorSelect($name)
		{
			return "<select name='$name' id='$name'>
				<option value='0'></option>
				<option value='>'> > </option>
				<option value='>='> >= </option>
				<option value='<'> < </option>
				<option value='<='> <= </option>
				<option value='='> = </option>
			</select>";
		}
	 ?>
