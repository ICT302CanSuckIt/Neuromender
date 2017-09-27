<html>
	<head>
		<meta charset="UTF-8">
		<meta http-equiv="X-UA-Compatible" content="IE-edge">
		<meta name="viewport" content="width=device-width, initial-scale-1.0">
		<title> Neuromender | Dashboard </title>
		<script src ="assets/js/jquery.min.js"> </script>
		<script src ="bootstrap/js/bootstrap.min.js"> </script>
		<script src ="assets/js/ie10-viewport-bug-workaround.js"> </script>
		<link href="bootstrap/css/bootstrap.css" rel="stylesheet">
		<link href="style.css" rel="stylesheet">
		<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
		<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300|Roboto:400,700,500,300|Lato:400,700,300|Montserrat:400,700|Source+Sans+Pro:400,300,700' rel='stylesheet' type='text/css' />
		<?php
			include "Includes/DBConnect.php";
			if ($_SESSION['loggedIn'] == false)
			{
				header("Location: Main/Login.php");
				exit();
			}
			if (isset($_SESSION['UserID']))
			{
				$User = $_SESSION['UserID'];
			}
			
			date_default_timezone_set('Australia/Perth'); //set to Perth timezone, by default php sets to UTC
			include "Includes/GenerateAlerts.php";
		?>
	</head>
  
	<body>
		<div class="main-wrapper">
	 
			<div class="main-header">
			<!--<nav class="navbar navbar-default" role="navigation">-->
				<!-- <div class="container-fluid"> -->
				<!-- <div class="navbar-header"> -->
				<button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
					<span class="sr-only">Toggle Navigation</span>
				</button>
				<a class="navbar-brand" >Neuromender</a>
		
				
				<div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1"> 
				
					<div class="nav navbar-nav">                           
						<li class="active">
							<a class="main-header-links" href="Dashboard.php"> <i class='fa fa-home fa-lg'> </i> Dashboard</a>
						</li>
						
						<li>
							<a class="main-header-links" href="Main/About.php"> <i class='fa fa-question fa-lg'> </i> About</a>
						</li>
						<li>
							<a class="main-header-links" href="Main/Help.php"> <i class='fa fa-child fa-lg'> </i> Help</a>
						</li>
						<li>
							<a class="main-header-links" href="Main/Contact.php"> <i class='fa fa-phone fa-lg'> </i> Contact Us</a>
						</li>
					</div>
								

			 
					<div class="nav navbar-nav navbar-right" style="display: block;">
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
									$output = $output . "<div class='main-header-links'>Logged in as: $RoleDesc </div>";
								}
								/*<i class='fa fa-user fa-lg'></i>
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
							<a class="main-header-links" href="Main/Logout.php" style="display: block;">
									<i class="fa fa-sign-out fa-lg"></i>
									&nbsp;Logout
							</a>
						</li>
					</div>
				</div>
			</div>

			<div class="main-wrapper">
				<div class="main-content">
					<div class="main-sidebar"> 
						<?php
							include "sidebar-dash.php"
						?>
					</div>
					<div class="jumbotron">
						<?php
						$User = $_SESSION['UserID'];
						$sql = "SELECT Users.FullName FROM Users WHERE Users.UserID = $User";
						$result = $dbhandle->query($sql);
						$user = $result->fetch_assoc();
						$FullName = $user['FullName'];
						$output;
						$output = "<h1 class='main-title'>Welcome $FullName</h1>";
						$output = $output . "<p class='para'>Please select your destination via the panels on the left sidebar</p>";
						echo $output;
						?>
					</div>
				</div>
			</div>
				
			<div class="main-footer">
				<?php 
					include "Includes/Footer_Dash.php"
				?>
			</div>
		</div>
	</body>
</html>
