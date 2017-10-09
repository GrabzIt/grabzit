<?php
if (!defined('ABSPATH')) exit; // Exit if accessed directly

if (!current_user_can('manage_options')) exit; // Should never get here

if(sanitize_text_field($_POST['grabzit_hidden']) == 'Y' && check_admin_referer('grabzit-admin')) 
{
    $grabzItKey = sanitize_text_field($_POST['grabzit_key']);
    update_option('grabzit_key', $grabzItKey);
}
else
{
    $grabzItKey = get_option('grabzit_key');
}
?>
<div class="wrap">
<h2>GrabzIt Web Capture Settings</h2>
<form method="POST" action="">
<table class="form-table">
<tbody>
<tr valign="top">
<th scope="row">
<?php
wp_nonce_field('grabzit-admin');
?>
<input type="hidden" name="grabzit_hidden" value="Y">
    <label for="grabzit_key">Application Key</label>
</th>
<td>
<input type="text" name="grabzit_key" value="<?php echo $grabzItKey ?>" class="regular-text">
<p class="description">Create a free GrabzIt account to get your <a target="_blank" href="https://grabz.it/login.aspx?action=create&returnurl=https%3a%2f%2fgrabz.it%2fapi%2f%23Key">application key</a><br/> and then copy it into the above textbox.</p>
</td>
</tr>
</tbody>
</table>
<p class="submit">
<input type="submit" name="Submit" value="Save settings" class="button-primary"/>
</p>
</form>
<h3>Getting Started</h3>
<p>First you must <a href="https://grabz.it/account/domains.aspx" target="_blank">authorize your domain</a> to ensure no one else can use your application key.</p>
<p>Then to create a capture you need to specify the grabzit tags around the content you wish to capure. For instance you could convert a URL into a screenshot or you could convert a HTML snippet into a image, as shown in the two examples below.</p>
<pre>
[grabzit]https://www.google.com[/grabzit]
[grabzit]&lt;h1&gt;Hello there&lt;/h1&gt;[/grabzit]
</pre>
<p>To further customize your captures just choose one of these <a href="https://grabz.it/api/javascript/parameters.aspx" target="_blank">available options</a> and then add the option as an attribute to the grabzit tag. In the below example the download attribute has been set to true and the format attribute has been set to PDF, which means a screenshot of google.com will be automatically downloaded as a PDF.</p>
<pre>
[grabzit format="pdf" download="1"]https://www.google.com[/grabzit]
</pre>
<p>If you have any questions please <a href="https://grabz.it/contact.aspx?subject=WordPress+Plugin+Support" target="_blank">ask our support team</a>.</p>
</div>
