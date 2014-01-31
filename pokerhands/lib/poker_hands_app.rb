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

	def determine_result(playerA, playerB)
		result = Result.new
		if playerA.rank.value > playerB.rank.value
			result.winner = playerA
			result.justification = result.winner.rank.to_s
		elsif playerA.rank.value < playerB.rank.value
			result.winner = playerB
			result.justification = result.winner.rank.to_s
		else
			case playerA.rank.value
				when 1
					my_sorted_cards = playerA.cards.sort
					other_sorted_cards = playerB.cards.sort

					my_sorted_cards.zip(other_sorted_cards).reverse_each { |e, f| 
						if(e > f)
							result.winner = playerA
							result.justification = "high card: #{e}"
							break
						elsif (e < f)
							result.winner = playerB
							result.justification = "high card: #{f}"
							break
						end	
					}  
				else
				  throw "DONT KNOW"
			end
		end
		
		result
	end
end

class Result
	attr_accessor :winner
	attr_accessor :justification
end	