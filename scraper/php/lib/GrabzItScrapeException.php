<?php
namespace GrabzIt\Scraper;

use Exception;

class GrabzItScrapeException extends \Exception {

    const SUCCESS = 0;
    const PARAMETER_MISSING_SCRAPE_ID = 158;
    const PARAMETER_INVALID_SCRAPE_ID = 159;
    const PARAMETER_MISSING_SCRAPE_STATUS = 160;
    const PARAMETER_INVALID_SCRAPE_STATUS = 161;
    const PARAMETER_INVALID_SCRAPE_STATUS_CHANGE = 162;
    const NETWORK_SERVER_OFFLINE = 200;
    const NETWORK_GENERAL_ERROR = 201;
    const NETWORK_DDOS_ATTACK = 202;
    const RENDERING_ERROR = 300;
    const RENDERING_MISSING_SCREENSHOT = 301;
    const GENERIC_ERROR = 400;
    const UPGRADE_REQUIRED = 500;
    const FILE_SAVE_ERROR = 600;
    const FILE_NON_EXISTANT_PATH = 601;

    public function __construct($message, $code)
    {
        $this->code = $code;
	    parent::__construct($message);
    }
}
?>