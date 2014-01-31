class PokerHandsApp
	def compare_hands(input)
		parser = PlayerParser.new
		# eg. "Black: 2H 3D 5S 9C KD White: 2C 3H 4S 8C AH"
		# should return two matches
		# "Black: 2H 3D 5S 9C KD"
		# and
		# "White: 2C 3H 4S 8C AH"
		pattern = /(\w+:(?:\s[2-9TJQKA][HDSC]){5})/
		playerA, playerB = input.scan(pattern).map { |e| parser.parse(e.first) }

		result = determine_result(playerA, playerB)		
				
		"#{result.winner.name} wins - #{result.justification}"
	end

	def high_card_compare(playerA, playerB)
		playerA_sorted_cards = playerA.cards.sort
		playerB_sorted_cards = playerB.cards.sort

		result = Result.new
		playerA_sorted_cards.zip(playerB_sorted_cards).reverse_each { |e, f| 
			if(e.figure != f.figure)
				result.winner = e > f ? playerA : playerB
				result.justification = "high card: #{[e, f].max}"
				return result
			end	
		}
	end

	def determine_result(playerA, playerB)	
		if playerA.rank.value != playerB.rank.value
			result = Result.new
			result.winner = [playerA, playerB].max_by(&:rank)
			result.justification = result.winner.rank.to_s
			return result
		else
			method_name = "#{playerA.rank.name.tr(' ', '_')}_compare"
			return self.send(method_name, playerA, playerB)
		end
	end
end

class Result
	attr_accessor :winner
	attr_accessor :justification
end	