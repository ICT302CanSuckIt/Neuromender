<html>
<head>
	        <script type='text/javascript' src='user.js'></script>
					<script type='text/javascript' src='profile.js'></script>
					<script type='text/javascript' src='patientprofile.js'></script>
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
            $url = "location.href='Profile.php?user=$userID&password=1'";
            echo("
			
			<i class='fa fa-user fa-3x' aria-hidden='true'> </i>
                <a href='javascript:void(0)' onclick=$url>
			
				
				
                    <div class='huge'>Profile</div>
					<a href='javascript:void(0)' onclick=$url>
					
					
						<div class='clearfix'></div>
					</a>
		
				</a>
				
            ");
			
	
			
			
            //DASHBOARD BUTTON FOR REPORTS
                if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
                {
                echo("
						<i class='fa fa-folder-open fa-3x'></i>                       
					   <a href='Reports.php'>
						
					
					
                            <div class='huge'>Reports</div>
                            <a href='Reports.php'>
								<div class='clearfix'></div>
							</a>
					
						</a>
                   
                    ");
                }
                
                if($_SESSION['SelectedRole'] == $constSuperAdmin)
                {
                    echo("
                    <!-- DASHBOARD BUTTON FOR CSV DATA DOWNLOAD -->
						<i class='fa fa-download fa-3x'></i>
                         <a href='Download.php'>
						
					
						
							<div class='huge'>Download Data</div>
							 <a href='Download.php'>
								<div class='clearfix'></div>
							</a>
					
                        </a>
                    ");
                }
			
			
		
		
			
			
			//DASHBOARD BUTTON FOR ALERTS
            $userID = $_SESSION['UserID'];
            $url = "location.href='Alerts.php?user=$userID&password=1'";
            echo("
				<i class='fa fa-exclamation fa-3x'></i>
                <a href='javascript:void(0)' onclick=$url>
				
			
				
                    <div class='huge'>Alerts</div>
                    <a href='javascript:void(0)' onclick=$url>
						<div class='clearfix'></div>
                    </a>
			
				</a>
           ");
			?>
			
