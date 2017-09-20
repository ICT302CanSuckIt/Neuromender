<?php
    date_default_timezone_set('Australia/Perth'); //set to Perth timezone, by default php sets to UTC
    include "../Includes/DBConnect.php";
    include "../Includes/Calendar.php";
    require_once "../Includes/phpChart_Professional/conf.php";
    if (isset($_SESSION['UserID']))
    {
        $User = $_SESSION['UserID'];
    }
	
	error_reporting(0); 
	
	if ($_SESSION['loggedIn'] == false)
	{
		header("Location: Login.php");
		exit();
	}
?>
<html>
    <head>
        <title> Neuromender | Profile </title>
		<script type='text/javascript' src='profile.js'></script>
		<script type='text/javascript' src='patientprofile.js'></script>
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

<body>
       

	
	<div class="main-header">
            
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                            <span class="sr-only">Toggle Navigation</span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                        </button>
                        <a class="navbar-brand">Neuromender </a>
                   
                  <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                        <ul class="nav navbar-nav">
                            <li>
                                <a class="main-header-links" href="../Dashboard.php"> <i class='fa fa-home fa-lg'> </i> Dashboard</a>
                            </li>                              
                         
                            <li> <a class="main-header-links" href="About.php"> <i class='fa fa-question fa-lg'> </i> About</a></li>
			    <li> <a  class="main-header-links" href="Help.php"> <i class='fa fa-child fa-lg'> </i> Help</a></li>
			    <li> <a  class="main-header-links" href="Contact.php"> <i class='fa fa-phone fa-lg'> </i> Contact Us</a></li>
				
                        </ul>
                        <ul class="nav navbar-nav navbar-right" style="display: block;">
			    <li>
                                <?php
                                    $output = "";
                                    $selectRoles = "Select count(*) as roleCount from AssignedRoles where UserID = $User";
                                    $roleCount = getval($dbhandle, $selectRoles);
                                    if (isset($_SESSION['SelectedRole']))
                                    {
                                            $Role = $_SESSION['SelectedRole'];

                                            $roleSQL = "Select Description from Role where RoleID = $Role";
                                            $RoleDesc = getval($dbhandle, $roleSQL);
                                            $output = $output . "<div class='main-header-links'>Logged in as: $RoleDesc</a>";
                                    }
				    /*
                                    if($roleCount > 1)
                                    {
                                            $sql = "Select Role.RoleID, Role.Description from AssignedRoles INNER JOIN Role on Role.RoleID = AssignedRoles.RoleID where UserID = $User";
                                            $output = $output . "<form method='post' style='display: inline;'>";
                                            $output = $output . CreateSelectBox($sql, 'roleChange', 'roleChange', 'RoleID', 'Description', '', $dbhandle);
                                            $output = $output . "<input type='submit' name='btnRoleChange' value='Change' /></form><br><br>";
                                    }
				    */
				    echo $output;
                                ?>
                            </li>
			    <li>
                                <a class="main-header-links" href="Logout.php" style="display: block;">
                                    <i class="fa fa-sign-out fa-lg">
                                    </i>
                                    &nbsp;Logout
                                </a>
                            </li>
                        </ul>
                    </div>
            
	
	 </div>
	
<div class="bar"> </div>






<div class="main-wrapper">

	
			<div class="profile-jumbotron">
                <?php
                    include "../Includes/ProfileData.php"   
                ?>
			</div>
			
			
			
				<div class="profile-sidebar" >
				<?php
					include "../sidebar-profile.php"
				?>
			</div>

		
				
			
			
			 	      
	
			
		


</div>
<div class="clear"></div>


	<div class="main-footer">
            <?php
                include "../Includes/Footer.php"
            ?>
    </div>
	

    </body>
	
</html>

