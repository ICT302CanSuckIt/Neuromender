<?php
    date_default_timezone_set('Australia/Perth'); //set to Perth timezone, by default php sets to UTC
    include "DBConnect.php";
    include "Calendar.php";
		// Removed old phpChart module. No Longer Required. Retained due to Client request.
    /*require_once "phpChart_Professional/conf.php";*/
    if (isset($_SESSION['UserID']))
    {
        $User = $_SESSION['UserID'];
    }   
?>
<html>
  <head>
		<!-- <meta> Tags -->
		<meta charset="UTF-8">
		<meta http-equiv="X-UA-Compatible" content="IE-edge">
		<meta name="viewport" content="width=device-width, initial-scale-1.0">
		<!-- stylesheet links -->
		<link href="../bootstrap/css/bootstrap.css" rel="stylesheet">
		<link href="../style.css" rel="stylesheet">
		<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
		<link href="../Includes/GridStyle.css" rel="stylesheet">
		<!-- script links -->
		<script src ="../assets/js/jquery.min.js"> </script>
		<script src ="../bootstrap/js/bootstrap.min.js"> </script>
		<script src ="../assets/js/ie10-viewport-bug-workaround.js"> </script>
		<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
		<script type='text/javascript' src='user.js'></script>
		<script src="../js/amcharts/amcharts.js" type="text/javascript"></script>
		<script src="../js/amcharts/serial.js" type="text/javascript"></script>
	</head>
</html>