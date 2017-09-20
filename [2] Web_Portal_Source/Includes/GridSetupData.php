<?php
    error_reporting(0);
    $n = 0;
    $output;
    $output = $output . "<form method='post'>";
    $output = $output . "<h2>Grid Setup</h2>";
    $output = $output . "<table>";
    $maxOrder = 5;
    for ($n = 1; $n <= 20; $n++)
    {
        $output = $output . "<tr>Grid " . $n;
        $output = $output . "<td>Order: <input type='number' name='answer' value='' min = '0' max = '".$maxOrder."'></td><td>Repetitions: <input type='number' name='answer' value='' min = '0'></td>";
        $output = $output . "</tr>";
    }
    $output = $output . "</table>";
    $output = $output . "<br><br><input type='submit' id='btnGridSetup' name='btnGridSetup'/>";
    $output = $output . "</form>";
    echo $output;
?>

