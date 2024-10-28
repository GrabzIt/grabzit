<?php
   /*
   Plugin Name: GrabzIt Web Capture
   Plugin URI: https://grabz.it
   Description: Use a simple shortcode to screenshot a webpage or convert any text or HTML snippet into images, rendered HTML, PDF's, DOCX, Animated GIF's, CSV, JSON and more! To get started: activate the GrabzIt Web Capture and go to the GrabzIt API page to get your Application Key. 
   Version: 1.0.8
   Author: GrabzIt Limited
   Author URI:   https://grabz.it/
   License: GPL2
   License URI:  https://www.gnu.org/licenses/gpl-2.0.html
   Text Domain: grabzit-web-capture
   Domain Path: /languages
   */

if (!defined('ABSPATH')) exit; // Exit if accessed directly

function grabzit_shortcodes_init()
{
    load_plugin_textdomain('grabzit-web-capture', false, dirname(plugin_basename(__FILE__)) . '/languages/');
    
    function grabzit_shortcode($atts, $content)
    {
        wp_enqueue_script('grabzit', plugin_dir_url(__FILE__) . 'grabzit.min.js');

        $id = 'grabzit' . uniqid();

        $grabzItKey = get_option('grabzit_key');

        $options = '{}';
        if ($atts !== null)
        {
            $options = json_encode($atts);
        }

        $script = '';
        if (filter_var($content, FILTER_VALIDATE_URL)) 
        { 
            //is a URL
            $script = 'GrabzIt("'.$grabzItKey.'").ConvertURL("'.grabzit_js_safe(esc_url_raw($content)).'", '.$options.').AddTo("'.$id.'");';
        }
        else
        {
            //is a HTML snippet
            $script = 'GrabzIt("'.$grabzItKey.'").ConvertHTML("'.grabzit_js_safe(wp_kses($content, wp_kses_allowed_html('post'))).'", '.$options.').AddTo("'.$id.'");';
        }
        
        wp_add_inline_script('grabzit', $script);
        
        return '<span id="'.$id.'"></span>';
    }
    
    add_shortcode('grabzit', 'grabzit_shortcode');
    
    function grabzit_js_safe($content)
    {
        if (!empty($content))
        {
            $content = addcslashes($content, '"');
            $content = str_replace(array("\n", "\r", "\t"), array("\\n", "\\r", "\\t"), $content);
        }
        return $content;
    }
    
    function grabzit_admin() 
    {
        include('grabzitplugin.admin.php');
    }    
    
    function grabzit_admin_actions() 
    {
        if (current_user_can('manage_options'))
        {
            add_options_page(__('GrabzIt', 'grabzit-web-capture'), __('GrabzIt', 'grabzit-web-capture'), 1, __('GrabzIt', 'grabzit-web-capture'), "grabzit_admin");
        }
    }
 
    add_action('admin_menu', 'grabzit_admin_actions');

	function grabzit_plugin_action_links( $links )
	{
		$links = array_merge( array(
			'<a href="' . esc_url( admin_url( '/options-general.php?page=GrabzIt' ) ) . '">' . __( 'Settings', 'textdomain' ) . '</a>'
		), $links );
		return $links;
	}    
	
	add_action('plugin_action_links_' . plugin_basename( __FILE__ ), 'grabzit_plugin_action_links');
}
add_action('init', 'grabzit_shortcodes_init');   
?>