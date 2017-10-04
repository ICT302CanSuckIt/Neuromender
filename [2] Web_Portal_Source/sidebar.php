<html>
<head>
	        <script type='text/javascript' src='user.js'></script>
					<script type='text/javascript' src='profile.js'></script>
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
            $url = "Profile.php?user=$userID&password=1";
            echo("<div class='sidebar-links' $hackedStyle>
                <a href=\"$url\">
			
				
				<i class='fa fa-user fa-3x'> </i>
                    <div class='huge'>Profile</div>
					<a href=\"$url\">
						<div class='clearfix'></div>
					</a>
				
				</a>
				</div>
            ");
			
	
			
			
            //DASHBOARD BUTTON FOR REPORTS
                if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
                {
                echo("<div class='sidebar-links' $hackedStyle>
                        <a href='Reports.php'>
						
						
						<i class='fa fa-folder-open fa-3x'></i>
                            <div class='huge'>Reports</div>
                            <a href='Reports.php'>
								<div class='clearfix'></div>
							</a>
						
						</a>
                    </div>
                    ");
                }
                
                if($_SESSION['SelectedRole'] == $constSuperAdmin)
                {
                    echo("
                    <!-- DASHBOARD BUTTON FOR CSV DATA DOWNLOAD -->
                    <div class='sidebar-links' $hackedStyle>
                         <a href='Download.php'>
						
						
						<i class='fa fa-download fa-3x'></i>
							<div class='huge'>Download Data</div>
							 <a href='Download.php'>
								<div class='clearfix'></div>
							</a>
						
                        </a>
                    </div>");
                }
			
			
			
         
			
		
			
			
						//DASHBOARD BUTTON FOR ALERTS
            $userID = $_SESSION['UserID'];
            $url = "Alerts.php?user=$userID";
            echo("<div class='sidebar-links' $hackedStyle>
                <a href=\"$url\">
				
				
				<i class='fa fa-exclamation fa-3x'></i>
                    <div class='huge'>Alerts</div>
                    <a href=\"$url\">
						<div class='clearfix'></div>
                    </a>
				
				</a>
				
            </div>");
						
						//DASHBOARD BUTTON FOR Notes
            $userID = $_SESSION['UserID'];
            $url = "Notes.php?user=$userID";
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
