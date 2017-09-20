<?php
	if ($_SESSION['loggedIn'] == false)
	{
		header("Location: Login.php");
		exit();
	}
	// PHP Settings
	ini_set('display_errors',1);
	error_reporting(0);
	
	//Uncomment when needed
	
	ini_set("SMTP", "network.murdoch.edu.au");
	ini_set("smtp_port", "25");
	ini_set("sendmail_from", "Neuromender@network.Murdoch.edu.au");
	
    $outputString;
    
	// Mail Variables
	$emailSubject = "";
	$emailBody = "";
	$emailRecipient = "";
	$emailHeader = "";
    
    $outputString = "<div class='body'>";
    $outputString = $outputString . "<h2>Contact Form</h2>";
	$outputString = $outputString . "<form name='form' action='' method='post'>";
    $outputString = $outputString . "<h3>Title</h3>";
	$outputString = $outputString . "<textarea name='titleText' rows='1' cols='100' style='resize:none;'></textarea><br>";
	$outputString = $outputString . "<h3>Reason for Contacting</h3>";
	$outputString = $outputString . "<textarea name='bodyText' rows='6' cols='100' style='resize:none;'></textarea><br>";
	$outputString = $outputString . "<h3>Your Name</h3>";
	$outputString = $outputString . "<textarea name='userText' rows='1' cols='100' style='resize:none;'></textarea><br><br>";
	$outputString = $outputString . "<input type='submit' value='Send Email' class='btn btn-primary btn-sm' name='btnSendEmail'>";
	$outputString = $outputString . "</form>";
	
	// Read the email addresses
	$handle = fopen("email.txt", "r");
	if ($handle)
	{
		while (($line = fgets($handle)) !== false)
		{
			$line = rtrim($line, "\r\n");
			$emailRecipient .= $line . ";";
		}
		
		fclose($handle);
	}
	
	// Set Variables and Send Email
	$emailFrom = "Neuromender@network.murdoch.edu.au";
	$emailSubject = $_POST['titleText'];
	$emailBody = $_POST['bodyText'] . "\n" . " -- Email Request is From: " . $_POST['userText'];
	$emailHeader  = 'MIME-Version: 1.0' . "\r\n";
	$emailHeader .= 'Content-type: text/html; charset=iso-8859-1' . "\r\n";
	$emailHeader .= 'From: ' . $emailFrom . "\r\n";
	$emailHeader .= 'Reply-To: ' . $emailFrom . "\r\n";
	$emailHeader .= 'X-Mailer: PHP/' . phpVersion();
	
	// Triggers after button press, and sends email if the fields weren't empty
	if(isset($_POST["btnSendEmail"]))
	{
		if ($emailSubject != "" && $emailBody != "")
		{
			mail($emailRecipient, $emailSubject, $emailBody, $emailHeader);
		}

	}
	
    $outputString = $outputString . "</div>";
	
	$outputString = "
	
	<h1 class='main-title'>Contact Us</h2><br/>
	
	<p class='para'>If you have an issue or need to contact us, please email Dr Shiratuddin at <a class='links' href='mailto:f.shiratuddin@murdoch.edu.au'>f.shiratuddin@murdoch.edu.au</a></p>
	<!--
	<table>
		<tr>
			<td>
				Dr Mohd Fairuz Shiratuddin
			<td>
			<td>
				f.shiratuddin@murdoch.edu.au
			<td>-->
	";
    echo $outputString;
?>