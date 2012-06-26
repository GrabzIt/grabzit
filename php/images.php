<html>
<head>
</head>
<body>
<div style="width:800px;">
<?php
if (!is_writable("images"))
{
    ?><span style="color:red;font-weight:bold;">The "images" directory is not writable! This directory needs to be made writable in order for this demo to work.</span><?php
    return;
}

$images = glob("images" . DIRECTORY_SEPARATOR . "*.*");

//print each file name
foreach($images as $image)
{
    if (strpos($image, ".txt") !== false)
    {
        continue;
    }

    echo "<img src='$image' style='margin-right:1em;'/>";
}
?>
</div>
</body>
</html>