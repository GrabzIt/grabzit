<html>
<head>
</head>
<body>
<div style="width:800px;">
<?php
$images = glob("images" . DIRECTORY_SEPARATOR . "*.*");

//print each file name
foreach($images as $image)
{
    if (strpos($image, ".txt.") !== false)
    {
        continue;
    }

    echo "<img src='$image' style='margin-right:1em;'/>";
}
?>
</div>
</body>
</html>