// ----------------------------------------------------------------------------
// ____
// by KingTomato
// --
// Abstract:
//   This is kinda cool map I and the GoDz team made (I coded, they gave some
// harsh, but very appreciated critisism). The map from a glance looks straight
// forward. You have two bases, a lot of bunkers, and a couple of nice camping
// spots on the sides. The ability to get on the wall is present, and neccesary
// to cap the objectives. What isn't present however, are the various triggers
// and tactics you can use for an easy (and decrative) win.
//   For instance, each large crate on your team's side (With the exception of
// the gray ones in the center, and brown just below the objectives) has a
// unique ability. These have teleports to give you the ability to switch your
// location instantaniously. What advantage does this have? Simple. The element
// of surprise! Using these efficiently means leaving the enemy with only a
// guess of where your next shot will come from.
//   In addition to the crate teleports, you also have triggers around the flag
// to promote the very essence of the game of paintball--teamwork. If one
// person (or even 2) attempt a flag capture, you are faced with grabbing the
// flag and returning the entire distance of base to base for the point. If you
// use 3 people, you get a very interesting advantage. With 3 people (two touch
// the black panels on either side of the flag, and one caps) the capper's then
// transported halfway back to base automatically.
// ...
// ----------------------------------------------------------------------------

//
// Group Trigger -- Enter
// Player enters the group trigger
//
function GroupTrigger::onEnter(%this, %obj)
{
	//messageAll(5, sprintf("Collision: %1 (%2) -> %3", %this, %this.num, %obj));
 
	// get client id
	%client	= Gamebase::getOwnerClient(%obj);

	// Check for a .num value
	if (%this.num == "")
		return;

	// Split the .num property up into words
	for (%w = 0; (%num[%w] = getWord(%this.num, %w)) != -1; %w++) { }

	//messageAll(5,sprintf("%1 -> %2 -> %3", %num[0], %num[1], %num[2]));

	//
	// Triggers for bunker transporting
	//
	if (%num[0] == "Jump")
	{
		// Just to make like easier, i used the position of the other
		// triggers as the new teleport positions. To stop it from going
		// into infinite loop, i have it ignore an on enter if they are
		// being teleported
		if (%obj.teleporting)
			return;

		// Teams
		%team       = %num[1];
		%team[East] = 0;
		%team[West] = 1;

		// Check for correct team
		if (Gamebase::getTeam(%obj) != %team[%team])
		{
			Client::SendMessage(%client, 0, "Wrong Team. You may only use your team's teleports.");
			return;
		}

		// Setup the list of possible new positions on a per-base
		// kind of setup. Each jump has an associated team, we use
		// this to decide where they go
		%teleport[East, 0] = "-292.402 -254.302 280.786";
		%teleport[East, 1] = "-284.273 -222.375 280.786";
		%teleport[East, 2] = "-292.305 -190.194 280.786";
		%teleport[East, 3] = "-311.563 -282.487 280.786";
		%teleport[East, 4] = "-279.871 -274.894 280.786";
		%teleport[East, 5] = "-252.356 -246.282 280.786";
		%teleport[East, 6] = "-252.328 -198.446 280.786";
		%teleport[East, 7] = "-280.094 -170.044 280.786";
		%teleport[East, 8] = "-308.62 -222.481 280.786";
		%teleport[East, 9] = "-311.967 -162.358 280.786";
		%teleport[East,10] = "-239.838 -290.866 280.786";
		%teleport[East,11] = "-239.958 -154.101 280.786";

		%teleport[West, 0] = "-368.878 -162.099 280.785";
		%teleport[West, 1] = "-428.113 -246.139 280.785";
		%teleport[West, 2] = "-368.478 -282.221 280.785";
		%teleport[West, 3] = "-388.136 -254.391 280.785";
		%teleport[West, 4] = "-400.347 -274.54 280.785";
		%teleport[West, 5] = "-371.821 -222.105 280.785";
		%teleport[West, 6] = "-396.168 -222.211 280.785";
		%teleport[West, 7] = "-428.085 -198.303 280.786";
		%teleport[West, 8] = "-388.039 -190.284 280.785";
		%teleport[West, 9] = "-439.954 -290.056 280.785";
		%teleport[West,10] = "-400.57 -169.692 280.785";
		%teleport[West,11] = "-439.874 -154.391 280.785";

		// just number of triggers on either side
		%triggers = 12;

		// This gets kind of tricky. Here, i am going to do just like
		// the spawn points do--test for players in other bunker locations.
		// This makes it so you don't teleport to a bunker where someone
		// already is, and get on top of them, or in them. That's easy to
		// do, but I also have to make the points random.
		// To do this, i randomize the above list (the one that corrisponds
		// to the teleport they are using) and then go through it one by one.

		// Randomize the list (use 20 iterations of switching two elements)
		for (%a = 0; %a < 20; %a++)
		{
			%r1 = floor(getRandom() * %triggers);
			%r2 = floor(getRandom() * %triggers);
			// prevent switching same blocks
			while (%r1 == %r2) { %r2 = floor(getRandom() * %triggers); }

			%temp = %teleport[%team,%r1];
			%teleport[%team,%r1] = %teleport[%team,%r2];
			%teleport[%team,%r2] = %temp;
		}

		for (%a = 0; %a < %triggers; %a++)
		{
			// Here I just decrease the height of the teleport by a little. because they
			// are above the player, using their position as a search point would be inaccurate.
			// Also when you lower it, it makes it easier to spawn the player there without
			// them triggering another one immediatly after. Granted I have a safety, but
			// it could create an infinite loop.
			%teleport[%team,%a] = Vector::Add(%teleport[%team,%a],"0 0 -3.25");

			// Look in this spot for a person
			%set = newObject("set",SimSet);

			if (containerBoxFillSet(%set,$SimPlayerObjectType,%teleport[%team,%a],2,2,4,0) == 0)
			{
				// remove set
				deleteObject(%set);

				// teleport them
				Item::SetVelocity(%obj, "0 0 0");
				Gamebase::setPosition(%obj, %teleport[%team,%a]);
				playSound(SoundBeaconUse,%teleport[%team,%a]);

				%obj.teleporting = true;
				schedule(%obj @ ".teleporting = false;", 2.0);

				// exit
				return;
			}

			deleteObject(%set);
		}

		// Nothing is empty, notify player
		Client::SendMessage(%client, 0, "All bunkers are full. Please try again later.");
	}

	//
	// Trigger for flag
	//
	else if (%num[0] == "Flag")
	{
		%team       = %num[2];
		%team[East] = 0;
		%team[West] = 1;

		// Check for the walls
		if (%num[1] == "Panel")
		{
			// This one is already triggered
			if (%obj.atFlagPanel)
				return;

			$flagTrigger[%team]++;
			%obj.atFlagPanel = true;

			if ($flagTrigger[%team] < 0)
				$flagTrigger[%team] = 0;

			schedule("$flagTrigger[" @ %team @ "]--;", 2.0);
			schedule(%obj @ ".atFlagPanel = false;", 2.0);
		}

		// on flagstand
		else if (%num[1] == "Trig")
		{
			%pos[East, 1] = "-217.08 -217.96 289.5";
			%pos[East, 2] = "-216.861 -226.917 289.5";

			%pos[West, 1] = "-463.639 -218.194 289.5";
			%pos[West, 2] = "-463.579 -227.104 289.5";

			%set = newObject("Set",SimSet);
			%count = containerBoxFillSet(%set,$SimPlayerObjectType,%pos[%team,1],2,2,4,0);
			deleteObject(%set);

			%set = newObject("Set",SimSet);
			%count += containerBoxFillSet(%set,$SimPlayerObjectType,%pos[%team,2],2,2,4,0);
			deleteObject(%set);

			// Check for a player in each spot
			if (%count < 2)
				return;

			// Return Areas...
			%return[0] = "-340 -250 277.25";
			%return[1] = "-340 -260 277.25";
			%return[2] = "-340 -270 277.25";
			%return[3] = "-340 -280 277.25";
			%return[4] = "-340 -290 277.25";
			%return[5] = "-340 -150 277.25";
			%return[6] = "-340 -160 277.25";
			%return[7] = "-340 -170 277.25";
			%return[8] = "-340 -180 277.25";
			%return[9] = "-340 -190 277.25";

			// number of returns
			%returns = 10;

			%r = floor(getRandom() * %returns);

			Gamebase::SetPosition(%obj, %return[%r]);
			playSound(SoundBeaconUse,%return[%r]);
		}

		// Trip
		else if (%num[1] == "Trip")
		{
			%pos[West] = "-212.88 -222.474 289.767";
			%pos[East] = "-467.591 -222.618 289.768";

			Gamebase::setPosition(%obj, %pos[%num[2]]);
		}
	}

	//
	// Kill
	//
	else if (%num[0] == "Kill")
	{
		Player::kill(%client);
		messageAll(1, Client::GetName(%client) @ " was killed for leaving boundry.");
	}
}