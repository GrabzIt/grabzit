<?php
class GrabzItScrape
{
  public $ID;
  public $Name;
  public $Status;
  public $NextRun;
  public $Results;
  
  public function __construct()
  {
      $this->Results = array();
  }
}
?>