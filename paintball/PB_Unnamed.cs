// ----------------------------------------------------------------------------
// PB_Unnamed
// --
// To be used with the map, made by KingTomato
// ----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// some basic settings

// This is how many bunkers and cargo boxes should be places when the map loads.
$Unnamed::Bunkers	= 150;

// This is the range where the bunkers/cargo should go. X Y Z
// Got these using map markers. They go from corner to corner, starting just
// in front of both of those "special bunkers".
$Unnamed::Range[1]	= "-463 -421 294";
$Unnamed::Range[2]	= "-182 -144 294";

// This is the list of objects we will use when we are deploying random shit.
// Watch out though, if you don't know what you are doing, this could be fatal.
%obj = 0;
$Unnamed::Obj[%obj++]	= "BElcargo1";
$Unnamed::Obj[%obj++]	= "DSlcargo1";
$Unnamed::Obj[%obj++]	= "SWlcargo1";
$Unnamed::Obj[%obj++]	= "lcargo2";
$Unnamed::Obj[%obj++]	= "scargo3";
$Unnamed::Obj[%obj++]	= "ebunker";
$Unnamed::Obj[%obj++]	= "bunker4";
$Unnamed::Obj[%obj++]	= "lrock1";
$Unnamed::Obj[%obj++]	= "lrock2";
$Unnamed::Obj[%obj++]	= "lrock3";
$Unnamed::Obj[%obj++]	= "lrock4";
$Unnamed::Obj[%obj++]	= "lrock5";
$Unnamed::Obj[%obj++]	= "lrock6";
$Unnamed::ObjCount = %obj;

// What axes would you like to rotate the objects on?
$Unnamed::Rot[X] = false;
$Unnamed::Rot[Y] = false;
$Unnamed::Rot[Z] = true;

// preset list of rotations
%rot = 0;
$Unnamed::Rot[%rot++] = "0.0";
$Unnamed::Rot[%rot++] = "1.57084";
$Unnamed::Rot[%rot++] = "3.14158";
$Unnamed::Rot[%rot++] = "-3.14158";
$Unnamed::Rot[%rot++] = "-1.57084";
$Unnamed::RotCount = %rot;

// Debugging
$Unnamed::Debug = false;

// ----------------------------------------------------------------------------
// functions

// initialize the map
// the way i have it setup is i have setup a range of "available" spots for
// bunkers and cargo boxes on the map. What i am basically doing is 
function Unnamed::OnInit()
{
	// We get a number from 1 to %range[Whatever]. Then, we add the
	// %low[Whatever] to it to get withing our allowed range.

	%low[X] = getWord($Unnamed::Range[1], 0);
	%low[Y] = getWord($Unnamed::Range[1], 1);
	%low[Z] = getWord($Unnamed::Range[1], 2);

	Unnamed::Debug("Low X,Y,Z: " @ %low[X] @ " " @ %low[Y] @ " " @ %low[Z]);

	%range[X] = getWord($Unnamed::Range[2], 0) - %low[X];
	%range[Y] = getWord($Unnamed::Range[2], 1) - %low[Y];
	%range[Z] = getWord($Unnamed::Range[2], 2) - %low[Z];

	Unnamed::Debug("Range X,Y,Z: " @ %range[X] @ " " @ %range[Y] @ " " @ %range[Z]);

	// now we generate the bunkers all throughout the map
	for (%a = 0; %a < $Unnamed::Bunkers; %a++)
	{
		// Get the random numbers
		%rand[X] = getRandom() * %range[X];
		%rand[Y] = getRandom() * %range[Y];
		%rand[Z] = getRandom() * %range[Z];

		Unnamed::Debug("Rand X,Y,Z: " @ %rand[X] @ " " @ %rand[Y] @ " " @ %rand[Z]);

		// add their low value to get actual coordinates
		%act[X] = %rand[X] + %low[X];
		%act[Y] = %rand[Y] + %low[Y];
		%act[Z] = %rand[Z] + %low[Z];

		%position = %act[X] @ " " @ %act[Y] @ " " @ %act[Z];
		Unnamed::Debug("Position: " @ %position);

		// now, to liven things up, add a random rotation
		%rotation = "";
		%axes     = "X Y Z";
		for (%b = 0; (%axis = getWord(%axes, %b)) != -1; %b++)
		{
			%rot = "1";
			if ($Unnamed::Rot[%axis])
				%rot = floor(getRandom() * $Unnamed::RotCount) + 1;
			%rotation = %rotation @ $Unnamed::Rot[%rot] @ " ";
		}

		Unnamed::Debug("Rotation: " @ %rotation);

		// Now, get a random shape
		%randShape = floor(getRandom() * $Unnamed::ObjCount) + 1;
		while (($Unnamed::Limit[%randShape] != "") && ($Unnamed::Limit[%randShape] < 1))
		{
			Unnamed::Debug("Getting new random shape...");
			// this will allow us to put limits on shapes
			%randShape = floor(getRandom() * $Unnamed::ObjCount) + 1;
		}
		if ($Unnamed::Limit[%randShape] != "")
			$Unnamed::Limit[%randShape]--;
		Unnamed::Debug("Shape:"@$Unnamed::Obj[%randShape]);

		// Now place the object
		%newObject = newObject("bunker" @ %a,"InteriorShape",$Unnamed::Obj[%randShape]@".dis",true);
		addToSet("MissionCleanup", %newObject);

		Gamebase::setPosition(%newObject, %position);
		Gamebase::setRotation(%newObject, %rotation);
	}
}

// ----------------------------------------------------------------------------
// Trigger functions

// Entry
function GroupTrigger::OnEnter(%this, %obj)
{
	Unnamed::Debug("GroupTrigger::OnEnter(" @ %this @ "," @ %obj @ ");");

	%client	= Gamebase::getOwnerClient(%obj);
	%shape	= %this.num;
	%name	= getWord(%shape, 0);

	Unnamed::Debug("Shape: " @ %shape);

	// This will handle the teleports
	if (%name == "Teleport")
	{
		%team = getWord(%shape, 1);
		if (%team == "Team0")
		{
			%loc[0] = "-173.029 -433.652 299.318";
			%loc[1] = "-173.119 -139.052 299.318";
			%vel = "-300 0 0";
		}
		else if (%team == "Team1")
		{
			%loc[0] = "-479.183 -139.074 299.318";
			%loc[1] = "-479.261 -433.599 299.318";
			%vel = "300 0 0";
		}

		%r = floor(getRandom() * 100) % 2;
		Gamebase::setPosition(%client, %loc[%r]);
		Player::applyImpulse(%client, %vel);
	}

	// Bunkers
	else if (%name == "Bunker")
	{
		%action = getWord(%shape, 1);
		if (%action == "Enter")
		{
			%pos = getWord(%shape, 2)@" "@getWord(%shape, 3)@" "@getWord(%shape, 4);

			Unnamed::Debug("Sending " @ %client @ " to " @ %pos);
			Gamebase::SetPosition(%client, %pos);
			Item::setVelocity(%client, "0 0 0");
			playSound(SoundActivatePda, %pos);

			// stop right here
			return;

			// this isn't working jsut yet. Keeps killing server. I'll figure out why later

			// First, we need to check for other players
			%range = 1;
			%set = newObject("set",SimSet);
			containerBoxFillSet(%set, $SimPlayerObjectType, %pos, %range, %range, %range);
			%num = Group::objectCount(%set);
			if (%num > 0)
			{
				%bPl = Group::getObject(%set, %i);
				if (!Player::isDead(%bpl))
				{
					Client::SendMessage(%client, 1, "There is already a player in the bunker~waccess_denied.wav");
					deleteObject(%set);
					return;
				}
			}
			deleteObject(%set);
		}
		else if (%action == "Exit")
		{
			// Check if they are lower then the trigger (or else they landed on it)
			%posThis = Gamebase::GetPosition(%this);
			%posObj = Gamebase::getPosition(%obj);

			if (getWord(%posThis, 2) > getWord(%posObj, 2)) // Below
			{
				%pos = Vector::Add(GameBase::GetPosition(%client), "0 0 5");
				Unnamed::Debug("Sending " @ %client @ " to " @ %pos);
				GameBase::setPosition(%client, %pos);
				Player::ApplyImpulse(%client, "0 0 300");
			}
			else // On Top
				// They get flown in the air. Make sure they don't kill themselves.
				Item::SetVelocity(%obj, "0 0 0");
		}
	}
}

// ----------------------------------------------------------------------------

function Unnamed::Debug(%text)
{
	if ($Unnamed::Debug)
		echo("Unnamed::Debug: " @ %text);
}

// Just used when i was map editing. I'll leave it though, so if you want to see
// how nice a job it does >:D
function Unnamed::ClearMap(%client)
{
	for (%obj = getNextObject("MissionCleanup", 0); %obj != 0; %obj = getNextObject("MissionCleanup", %obj))
	{
		if (getObjectType(%obj) == InteriorShape)
			deleteObject(%obj);
	}
}

Unnamed::onInit();