class PokerHandsApp
	def compare_hands(input)
		parser = PlayerParser.new
		# eg. "Black: 2H 3D 5S 9C KD White: 2C 3H 4S 8C AH"
		# should return two matches
		# "Black: 2H 3D 5S 9C KD"
		# and
		# "White: 2C 3H 4S 8C AH"
		pattern = /(\w+:(?:\s[2-9TJQKA][HDSC]){5})/
		players = input.scan(pattern).map { |e| parser.parse(e.first) }

		winner = players.max_by(&:rank)
		"#{winner.name} wins - #{winner.rank}"

	end
end