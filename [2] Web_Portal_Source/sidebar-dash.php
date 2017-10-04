<html>
    <head>
			<title> Neuromender | Profile </title>
			<script type='text/javascript' src='profile.js'></script>
			<meta charset="UTF-8">
			<meta http-equiv="X-UA-Compatible" content="IE-edge">
			<meta name="viewport" content="width=device-width, initial-scale-1.0">
			<link href="../bootstrap/css/bootstrap.css" rel="stylesheet">
			<link href="../style.css" rel="stylesheet">
			<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
			<link href="../Includes/GridStyle.css" rel="stylesheet">
			<script src ="../assets/js/jquery.min.js"> </script>
			<script src ="../bootstrap/js/bootstrap.min.js"> </script>
			<script src ="../assets/js/ie10-viewport-bug-workaround.js"> </script>
			<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
			<script type='text/javascript' src='user.js'></script>
			<script src="../js/amcharts/amcharts.js" type="text/javascript"></script>
			<script src="../js/amcharts/serial.js" type="text/javascript"></script>
			<link href="../forms.css" rel="stylesheet">
    </head>
</html>	
<!-- DASHBOARD BUTTON FOR PROFILE -->
		<?php
			
			$hackedStyle = ""; //This is a style used if the user is super admin, so that all the tiles fit on the screen. Hackity hack.
			if($_SESSION['SelectedRole'] == $constSuperAdmin)
			{
				$hackedStyle = "style='width:20%'";
			}
			
			$userID = $_SESSION['UserID'];
			$url = "./Main/Profile.php?user=$userID&password=1";
			echo("<div class='sidebar-links' $hackedStyle>
						<a href=\"$url\">				
							<i class='fa fa-user fa-3x'> </i>
							<div class='huge'>Profile</div>
						</a>
					</div>");

					//DASHBOARD BUTTON FOR REPORTS
					if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
					{
						echo("<div class='sidebar-links' $hackedStyle>
										<a href='Main/Reports.php'>
											<i class='fa fa-folder-open fa-3x'></i>
											<div class='huge'>Reports</div>
											<a href='Main/Reports.php'>
												<div class='clearfix'></div>
											</a>
										</a>
									</div>");
					}

					if($_SESSION['SelectedRole'] == $constSuperAdmin)
					{
						echo("
							<!-- DASHBOARD BUTTON FOR CSV DATA DOWNLOAD -->
							<div class='sidebar-links' $hackedStyle>
								<a href='Main/Download.php'>
									<i class='fa fa-download fa-3x'></i>
									<div class='huge'>Download Data</div>
									<a href='Main/Download.php'>
										<div class='clearfix'></div>
									</a>
								</a>
							</div>");
					}

					//DASHBOARD BUTTON FOR ALERTS
					$userID = $_SESSION['UserID'];
					$url = "Main/Alerts.php?user=$userID";
					echo("<div class='sidebar-links' $hackedStyle>
									<a href=\"$url\">
										<i class='fa fa-exclamation fa-3x'></i>
                    <div class='huge'>Alerts</div>
                    <a href='javascript:void(0)' onclick=\"$url\">
											<div class='clearfix'></div>
                    </a>
									</a>
							</div>");
							
						
						//DASHBOARD BUTTON FOR Notes
            $userID = $_SESSION['UserID'];
            $url = "./Main/Notes.php?user=$userID";
            echo("<div class='sidebar-links' $hackedStyle>
                <a href=\"$url\">
				
				
				<i class='fa fa-edit fa-3x'></i>
                    <div class='huge'>Notes</div>
                    <a href=\"$url\">
						<div class='clearfix'></div>
                    </a>
				
				</a>
				
            </div>");
		?>
