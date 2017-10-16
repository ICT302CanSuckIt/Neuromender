<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
<script type='text/javascript' src='user.js'></script>
<script type='text/javascript' src='../Includes/GridSelection.js'></script>

<?php
		if ($_SESSION['loggedIn'] == false)
		{
				header("Location: Login.php");
				exit();
		}
		error_reporting(0);
		$outputString;
		$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) 
		{
			
			//[Delete Note] stuff comes before the NoteID is reset.
			if(isset($_POST['delete'])){
				$deletedNote = $_SESSION['noteNum'];
				$sql = "DELETE FROM sessionnotes WHERE NoteID = ". $deletedNote;
				if (mysqli_query($dbhandle, $sql)) {
					//deleted successfully
					unset($_SESSION['noteNum']);
				} else {
					//delete failed. print error message.	
					echo "Error deleting record: " . mysqli_error($conn);
				}
			}
			
			//[Save Note] stuff comes before NoteID reset.
			//Users can save over existing notes.
			if(isset($_POST['save'])){
				$string = $_POST['notesText'];
				if(isset($_SESSION['noteNum'])){
					$sql = "UPDATE sessionnotes SET UserNote = \"". $string . "\" WHERE NoteID = ". $_SESSION['noteNum'];
					if (mysqli_query($dbhandle, $sql)) {
						//save successful
					} else {
						//save failed.
						echo "Error: " . $sql . "<br>" . mysqli_error($conn);
					}
				} else {
					$sql = "INSERT INTO sessionnotes (UserID, UserNote, Date) VALUES (\"". $User ."\", \"". $string . "\", NOW())" ;
					if (mysqli_query($dbhandle, $sql)) {
						//save successful
					} else {
						//save failed.
						echo "Error: " . $sql . "<br>" . mysqli_error($conn);
					}
				}
			}
			
			//reset Session Var
			$_SESSION['noteNum'] = "";
			unset($_SESSION['noteNum']);
			
			//stores user stuff into session store
			if(isset($_GET['user'])){
				$User = $_GET['user'];              
				$_SESSION['PatientID'] = $User;
			}
			
			//[New] button has been pressed
			if(isset($_POST['new'])){
				$_SESSION['noteNum'] = ""; //yes this is redundant, but safety first - BL
				unset($_SESSION['noteNum']);
			} else {
				if(isset($_POST['memoFile'])){
					$_SESSION['noteNum'] = intval($_POST['memoFile']);
				}
			}
			
			if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
			{
				
				echo '<h1 style="text-align:center;">';
				if(!isset($_SESSION['noteNum'])){
					echo "New Note";
				} else {
					echo "Viewing Note " . $_SESSION['noteNum'];
				}
				echo '</h1>';
?>
				<!--table holding 2 columns: 1 for notes menu, one for notes-->
				<table>
					<tr>
						<td>
							<!--//column holds notes menu (load/save)-->
							<form method="post">
								<select name="memoFile" style="width:auto;">
									<?php
										$sql = "Select * FROM sessionnotes WHERE UserID =" . $_SESSION['PatientID'];
										$result = mysqli_query($dbhandle,$sql);
										if($result->num_rows > 0){
											while($row = mysqli_fetch_assoc($result)){
												echo ("<option value='".$row['NoteID']."'>".date('d-m-Y G:i', strtotime($row['Date']))."</option>");
											}
										} else {
											echo ("<option selected='selected' disabled='disabled'>No Existing Notes</option>");
										}
									?>
								</select>
								<?php
									if($result->num_rows > 0){
										echo ("<br/><input type='submit' class='btn btn-primary btn-sm' style='top:10px; margin-bottom:0px;' value='Load'></input>");
									}
								?>
							</form>
							<?php
								//if the user is viewing their own notes they can add/save/delete
								if($_SESSION['UserID'] == $_SESSION['PatientID']){
									echo ("
										<input type='hidden' name='save' value='save' form='textBlock'></input>
									");
									if(!isset($_SESSION['noteNum'])){
										echo "<input type='submit' class='btn btn-primary btn-sm' value='Save New' form='textBlock'></input>";
									} else {
										echo "<input type='submit' class='btn btn-primary btn-sm' value='Save Changes' form='textBlock'></input>";
									}
									echo '</h1>';
									
									if(isset($_POST["memoFile"]) && ($_SESSION['UserID'] == $_SESSION['PatientID'])){
										echo ("<form method='post'>
											<input type='hidden' name='new' value='new'></input>
											<input type='submit' class='btn btn-primary btn-sm' value='New'></input>
										</form>
										
										<form method='post'>
											<input type='hidden' name='delete' value='delete'></input>
											<input type='submit' class='btn btn-primary btn-sm' value='Delete'></input>
										</form>
									");
									}
								}
							?>
						</td>
						<td>
							<form method="post" id="textBlock">
								<!--//column holds notes text area (type here)-->
								<textarea name="notesText" rows="20" cols="100" form="textBlock"><?php
									$notefound = 0;
									//loads note from database
									if(isset($_POST['memoFile'])){
										//grabs from DB by username instead
										$sql = "Select * FROM sessionnotes WHERE UserID=". $_SESSION['PatientID'];
											$result = mysqli_query($dbhandle,$sql);
											if($result->num_rows > 0){
												while($row = mysqli_fetch_assoc($result)){
													//refines result by NoteID here. wasn't working otherwise.
													if($row['NoteID'] == $_SESSION['noteNum']){
														echo $row['UserNote'];
														$notefound = 1;
													}
												}
												if($notefound == 0){//noteID not found
													echo ("ERROR! Note Could Not Be Loaded.");
												}
											} else {//if no notes by this user found.
												if(isset($_POST['notesText'])){//for when "save" is pressed
													echo $_POST['notesText'];
												} else{
													echo ("ERROR! Note Could Not Be Loaded.");
												}
											}
									}
								?></textarea>
							</form>
						</td>
					<tr>
				</table>
			<?php
			} else {//end of has viewing rights; else has no rights to this.
				$outputString = $outputString .  '<p style="color:red; font-size:150%">Error! You do not have permission to view this user.</p></div> '; 
			}
		} else {
			$outputString = $outputString .  '<p>Not Logged In</p></div> '; 
		}
		echo $outputString;
	 ?>
