<!DOCTYPE html>
<html>
	<head>
		<title>Test</title>
		<meta charset="UTF-8"></meta>
	</head>
	<body>
		<?php
			$key = openssl_random_pseudo_bytes(10);
			$k2 = $key . "  ";
			$fname ="../Includes/DBData.txt";
			$file = fopen($fname, "w");
			fwrite($file, $k2);
			
			$cipher = "AES-256-OFB";
			
			if (in_array($cipher, openssl_get_cipher_methods()))
			{
				echo "encrypting with: " . $cipher . "<br/>";
				echo "key: " . $key . "<br/>";
				$ivlen = openssl_cipher_iv_length($cipher);
				$iv = openssl_random_pseudo_bytes($ivlen);
				$iv2 = $iv . "  ";
				echo "iv: " . $iv . "<br/>";
				fwrite($file, $iv2);
			
				$filename = "../Includes/DBDataOLD.txt";
				$DBFile = fopen($filename, "r") or die ("Unable to Open File.");
				
				while(!feof($DBFile)){
					$tmp = rtrim(fgets($DBFile));
					if(($temp = openssl_encrypt(strval($tmp), $cipher, $key, $options=0, $iv)) != FALSE){
						$tmp2 = $temp . "  ";
						fwrite($file, $tmp2);
						
						echo ($temp . "<br/>");
					} else {
						echo "encrypt error. <br/>";
					}
				}
				
				fclose($DBFile);
			}
			
			fclose($file);
		?>
	</body>
</html>