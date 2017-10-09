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
<h2><?php _e('GrabzIt Web Capture Settings', 'grabzit-web-capture');?></h2>
<form method="POST" action="">
<table class="form-table">
<tbody>
<tr valign="top">
<th scope="row">
<?php
wp_nonce_field('grabzit-admin');
?>
<input type="hidden" name="grabzit_hidden" value="Y">
    <label for="grabzit_key"><?php _e('Application Key', 'grabzit-web-capture');?></label>
</th>
<td>
<input type="text" name="grabzit_key" value="<?php echo $grabzItKey ?>" class="regular-text">
<p class="description"><?php _e('Create a free GrabzIt account to get your', 'grabzit-web-capture');?> <a target="_blank" href="https://grabz.it/login.aspx?action=create&returnurl=https%3a%2f%2fgrabz.it%2fapi%2f%23Key"><?php _e('application key', 'grabzit-web-capture');?></a><br/><?php _e('and then copy it into the above textbox.', 'grabzit-web-capture');?></p>
</td>
</tr>
</tbody>
</table>
<p class="submit">
<input type="submit" name="Submit" value="Save settings" class="button-primary"/>
</p>
</form>
<h3><?php _e('Getting Started', 'grabzit-web-capture');?></h3>
<p><?php _e('First you must', 'grabzit-web-capture');?> <a href="https://grabz.it/account/domains.aspx" target="_blank"><?php _e('authorize your domain', 'grabzit-web-capture');?></a> <?php _e('to ensure no one else can use your application key.', 'grabzit-web-capture');?></p>
<p><?php _e('Then to create a capture you need to specify the grabzit tags around the content you wish to capture. For instance you could convert a URL into a screenshot or you could convert a HTML snippet into a image, as shown in the two examples below.', 'grabzit-web-capture');?></p>
<pre>
[grabzit]https://www.google.com[/grabzit]
[grabzit]&lt;h1&gt;<?php _e('Hello there', 'grabzit-web-capture');?>&lt;/h1&gt;[/grabzit]
</pre>
<p><?php _e('To further customize your captures just choose one of these', 'grabzit-web-capture');?> <a href="https://grabz.it/api/javascript/parameters.aspx" target="_blank"><?php _e('available options', 'grabzit-web-capture');?></a> <?php _e('and then add the option as an attribute to the grabzit tag. In the below example the download attribute has been set to true and the format attribute has been set to PDF, which means a screenshot of google.com will be automatically downloaded as a PDF.', 'grabzit-web-capture');?></p>
<pre>
[grabzit format="pdf" download="1"]https://www.google.com[/grabzit]
</pre>
<p><?php _e('If you have any questions please', 'grabzit-web-capture');?> <a href="https://grabz.it/contact.aspx?subject=WordPress+Plugin+Support" target="_blank"><?php _e('ask our support team', 'grabzit-web-capture');?></a> <?php _e('or read about', 'grabzit-web-capture');?> <a href="https://grabz.it/plugins/wordpress/" target="_blank"><?php _e('what else', 'grabzit-web-capture');?></a> <?php _e('you can do with the GrabzIt plugin.', 'grabzit-web-capture');?></p>
</div>
