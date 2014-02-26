<?php
$results= glob("../results" . DIRECTORY_SEPARATOR . "*.*");

if (!$results)
{
    return;
}

$output = array();
//print each file name
foreach($results as $result)
{
    if (strpos($result, ".txt") !== false)
    {
        continue;
    }

    $output[] = str_replace('../', '', $result);
}

echo json_encode($output);
?>