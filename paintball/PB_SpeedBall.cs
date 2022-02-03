messageall(3,"Map Made By GoHaN");

function GroupTrigger::onEnter(%this, %object)
{
	%client = Player::getClient(%object);
	if(%this.num == "Main1")
	{
		%object.shieldStrength = 500.0;
		schedule(%object @ ".shieldStrength = 0.0;" , 5);
		Client::SendMessage(%client,3,"You Have Entered The Battle Field!~wteleport2.wav");
		%baknue = floor(getRandom() * 9);
		if (%baknue == 0)
		{
			%positionIn = "50.5133 439.706 100";
			%Rot = "0 0 0";
		        %positionOut = "";
	      	}
		else if (%baknue == 1)
		{
		       %positionIn = "68.4123 438.895 100";
		       %Rot = "0 0 0";
		       %positionOut = "";
	      	}      	
		else if (%baknue == 2)
		{
			%positionIn = "82.4073 439.105 100";
			%Rot = "0 0 0";
			%positionOut = "";
		}
		else if (%baknue == 3)
		{
			%positionIn = "99.6131 439.203 100";
			%Rot = "0 0 0";
			%positionOut = "";
	      	}
		else if (%baknue == 4)
		{
			%positionIn = "114.133 438.304 100";
			%Rot = "0 0 0";
			%positionOut = "";
	      	}        	
		else if (%baknue == 5)
		{
			%positionIn = "131.016 437.414 100";
	  		%Rot = "0 0 0";
			%positionOut = "";
	      	}        	
	    	else if (%baknue == 6)
		{
			%positionIn = "96.3034 423.708 100";
	  		%Rot = "0 0 0";
			%positionOut = "";
	      	}        	
	     	else if (%baknue == 7)
		{
			%positionIn = "26.7042 436.572 100";
	  		%Rot = "0 0 0";
			%positionOut = "";
	      	}        	
	    	else if (%baknue == 8)
		{
			%positionIn = "153.064 437.679 100";
			%Rot = "0 0 0";
			%positionOut = "";
	      	}        	
	    	else if (%baknue == 9)
		{
			%positionIn = "81.9639 423.006 100";
			%Rot = "0 0 0";
			%positionOut = "";
		}
	}
	else if(%this.num == "Main2")
	{
		%object.shieldStrength = 500.0;
		schedule(%object @ ".shieldStrength = true;" , 5);
		Client::SendMessage(%client,3,"You Have Entered The Battle Field!~wteleport2.wav");
		%tort2 = floor(getRandom() * 9);
		if (%tort2  == 0)
		{
			%positionIn = "129.189 643.63 100";
			%Rot = "0 -0 3.0754";
	        	%positionOut = "";
	      	}
		else if (%tort2  == 1)
		{
			%positionIn = "110.717 644.203 100";
			%Rot = "0 -0 3.07539";
			%positionOut = "";
	      	}      	
		else if (%tort2  == 2)
		{
			%positionIn = "153.046 643.944 100";
			%Rot = "0 -0 3.07538";
			%positionOut = "";
	      	}   
		else if (%tort2  == 3)
		{
			%positionIn = "97.2774 644.316 100";
			%Rot = "0 -0 3.07537";
			%positionOut = "";
	      	}
		else if (%tort2  == 4)
		{
			%positionIn = "79.1351 644.718 100";
			%Rot = "0 -0 3.07536";
			%positionOut = "";
	      	}        	
		else if (%tort2  == 5)
		{
			%positionIn = "66.0604 645.367 100";
			%Rot = "0 -0 3.07535";
			%positionOut = "";
		}        	
		else if (%tort2  == 6)
		{
			%positionIn = "48.4913 646.489 100";
			%Rot = "0 -0 3.07534";
			%positionOut = "";
		}        	
		else if (%tort2  == 7)
		{
			%positionIn = "27.5013 643.981 100";
			%Rot = "0 -0 3.07533";
			%positionOut = "";
		}        	
		else if (%tort2  == 8)
		{
			%positionIn = "83.7321 658.373 100";
			%Rot = "0 -0 3.07532";
			%positionOut = "";
		}        	
		else if (%tort2  == 9)
		{
			%positionIn = "98.3797 658.007 100";
			%Rot = "0 -0 3.07531";
			%positionOut = "";
		}        	
	}
	if(%this.in){
		GameBase::setPosition(%client, %positionIn);
		GameBase::setRotation(%client, %Rot);         
	}
	else if(%this.out){
		GameBase::setPosition(%client, %positionOut);
        }
}

function TowerSwitch::onCollision(%this, %object) {
	if(GameBase::getMapName(%this) == "Need Help? Touch Me...")
	{
		%playerClient = Player::getClient(%object);

		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wsensor_deploy.wav\");", 0);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>Welcome to the <f2>Speedball <f1>information center!\", 4);", 0);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 3);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>How to play <f2>Speedball<f1>...\", 4);", 3);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 6);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>get weapons from invo and go through tele...\", 4);", 6);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 9);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>shoot enemy down with the paintball gun...\", 4);", 9);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 12);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>hide behind bunkers for protection...\", 4);", 12);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 15);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>while defeating enemy, try to capture the flag.\", 4);", 15);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 18);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>That is how you play <f2>Speedball<f1>.\", 4);", 18);
		schedule("Client::sendMessage(" @ %playerClient @ ", 1,\"~wshell_click.wav\");", 21);
		schedule("bottomprint(" @ %playerClient @ ", \"<jc><f1>Map By <f2>GoHaN<f1>.\", 4);", 21);
	}
}