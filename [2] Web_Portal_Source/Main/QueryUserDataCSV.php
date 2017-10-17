<?php
    error_reporting(0);
    include ("../Includes/DBConnect.php");
      
	if ($_SESSION['loggedIn'] != true) {
        $outputString = $outputString . "<p>Not logged in</p></div></div>";
        return;
    }

    if($_SESSION['SelectedRole'] != $constSuperAdmin)
    {
        $outputString = $outputString . "<p>Insufficient permissions</p></div></div>";
        return;
    }

    $selectUsers = $_POST['selectUsers'];
    
    $sql = "SELECT * FROM Users WHERE UserID = $selectUsers";
	  $result = $dbhandle->query($sql);
		if($result->num_rows > 0){
			$row = $result->fetch_assoc();
			$username = $row["Username"];
			$username = str_replace(' ', '', $username);

		
			
			$fileSize = 0;
			$files = array();
			$filesContent = array();
			
			$csvContent = array();
			$csvContent[] = array(
					"filename" => __DIR__ . "../../../../tmp/WingmanTracking.csv",
					"sqlQuery"=>file_get_contents("./Queries/QueryUserRawTracking.txt"),
					"replaceTokens" => array(
							"{UserID}" => $selectUsers
					)
			);

			foreach($csvContent as $csv) {
					$filename = $csv["filename"];
					$query = $csv["sqlQuery"];
					$replacementTokens = $csv["replaceTokens"];
					
					foreach($replacementTokens as $key => $value) {
							$query = str_replace($key, $value, $query);
					}
					$result = $dbhandle->query($query, MYSQLI_USE_RESULT);
					$tempCsvFile = createFile($filename, $result);
					
					array_push($files, $tempCsvFile);
					array_push($filesContent, $filename);
			}
			/*foreach ($files as $file) {
					unlink($file);
		}*/
		//------------------------------------------//
		//Reach game data//
		//------------------------------------------//
			$fileSize = 0;
			$files = array();
			
			$csvContent = array();
			$csvContent[] = array(
				"filename" => __DIR__ . "../../../../tmp/TargetsGameData.csv",
				"sqlQuery"=>file_get_contents("./Queries/QueryUserReachGameData.txt"),
				"replaceTokens" => array(
					"{UserID}" => $selectUsers
				)
			);

			foreach($csvContent as $csv) {
				$filename = $csv["filename"];
				$query = $csv["sqlQuery"];
				$replacementTokens = $csv["replaceTokens"];
				
				foreach($replacementTokens as $key => $value) {
					$query = str_replace($key, $value, $query);
				}
				$result = $dbhandle->query($query, MYSQLI_USE_RESULT);
				$tempCsvFile = createFile($filename, $result);
				
				array_push($files, $tempCsvFile);
				array_push($filesContent, $filename);
			}
		//------------------------------------------//
		//Reach Tracking data//
		//------------------------------------------//
			$fileSize = 0;
			$files = array();
			
			$csvContent = array();
			$csvContent[] = array(
				"filename" => __DIR__ . "../../../../tmp/TargetsTrackingData.csv",
				"sqlQuery"=>file_get_contents("./Queries/QueryUserReachTrackingData.txt"),
				"replaceTokens" => array(
					"{UserID}" => $selectUsers
				)
			);

			foreach($csvContent as $csv) {
				$filename = $csv["filename"];
				$query = $csv["sqlQuery"];
				$replacementTokens = $csv["replaceTokens"];
				
				foreach($replacementTokens as $key => $value) {
					$query = str_replace($key, $value, $query);
				}
				$result = $dbhandle->query($query, MYSQLI_USE_RESULT);
				$tempCsvFile = createFile($filename, $result);
				
				array_push($files, $tempCsvFile);
				array_push($filesContent, $filename);
			}
			
		//------------------------------------------//
		//Rowing Tracking data code goes here//
		//------------------------------------------//

			ob_end_clean();
		
			useZipArchive($filesContent, $username);
		
			foreach ($files as $file) {
					unlink($file);
			}
		} else{
			echo ("
				<script>
					alert('Error! No Results Found!');
					window.location.href='./Download.php';
				</script>
			");
		}

    function createFile($filename, $content) {
        $output = fopen($filename, "w");
        $fields = $content->field_count;
        $header = "";
        $body = "";
        $count = 0;
        $headerarray = array();
        while ($row = mysqli_fetch_assoc($content)) {
            $array = array();
            foreach($row as $key => $value) {
                if($count == 0) {
                    array_push($headerarray, "'".$key."'");
                }
                array_push($array, "'".$value."'");
            }
            $body .= "\n";
            if($count == 0) {
                fputcsv($output, $headerarray);
            }
            $count++;
            fputcsv($output, $array);
        }
        mysqli_free_result($content);
        fclose($output);
				
		
		$fileSize = (filesize($filename) + 1333);
		
        return $filename;
    }
    
    function useZipArchive($fileCollection, $username){
        $zip = new ZipArchive();
    
        $tmp_file = tempnam('.','');
        $zip->open($tmp_file, ZipArchive::CREATE);
		
        foreach($fileCollection as $file){
            $download_file = file_get_contents($file);
            $zip->addFromString(basename($file),$download_file);
        }
    
        $zip->close();
		header('Content-disposition: attachment; filename=Neuromender-'.$username.'-Data.zip');
		header('Content-type: application/zip');
		header('Content-length: ' . filesize($tmp_file));
        readfile($tmp_file);
        unlink($tmp_file);
    }
    
?>