on run {input, parameters}
	set currentApp to input as text
	tell application "FreeTube" to activate
	tell application "System Events"
		delay 0.1
				key code 123
				delay 0.01
				key code 123
	end tell
	end
	tell application currentApp to activate
end run