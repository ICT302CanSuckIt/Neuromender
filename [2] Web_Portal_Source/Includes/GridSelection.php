<?php
	echo '<script type="text/javascript" src="../Includes/GridSelection.js"></script>';
	if( !is_numeric($gridSizeRow) || !is_numeric($gridSizeRow) )
	{
		$gridOrder = '1';
		$gridSizeRow = 0;
		$gridSizeCol = 0;
	}
	echo '<script type="text/javascript">GenerateGrid( "'.$gridOrder.'", '.$gridSizeRow.', '.$gridSizeCol.' );</script>';
?>