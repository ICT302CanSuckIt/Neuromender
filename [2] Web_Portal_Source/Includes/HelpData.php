<?php
	if ($_SESSION['loggedIn'] == false)
	{
		header("Location: Login.php");
		exit();
	}
	error_reporting(0);
	
	
	//include ("faq.html");
	echo "<h1 class='main-title'>Help</h1>";
	echo "<p class='subpara'>Below are the user manuals that will help you throughout the game.</p>";
	if($_SESSION['SelectedRole'] <= $constSuperAdmin)
	{
		echo '<a class="links-two" href="../Main/Help.php?manual=Web_portal_Super_Admin">Super Admin user manual</a><br/>';
	}
	
	if($_SESSION['SelectedRole'] <= $constAdmin)
	{
		echo '<a class="links-two" href="../Main/Help.php?manual=Web_portal_Admin">Admin user manual</a><br/>';
	}
	
	if($_SESSION['SelectedRole'] <= $constPhysio)
	{
		echo '<a class="links-two" href="../Main/Help.php?manual=Web_portal_Clincian_and_Coach">Clinician user manual</a><br/>';
	}
	
	if($_SESSION['SelectedRole'] <= $constPatient)
	{
		echo '<a class="links-two" href="../Main/Help.php?manual=Wingman_targets_Manual">Survivor Game user manual</a><br/>';
		echo '<a class="links-two" href="../Main/Help.php?manual=Web_portal_Survivor">Web portal survivor user manual</a><br/>';
		
	}
	
	if (isset($_GET["manual"]))
	{
		$manual = $_GET["manual"];
		include( "../assets/" . $manual . ".html");
	}
?>