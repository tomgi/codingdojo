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

		result = determine_result(players)		
				
		"#{result.winner.name} wins - #{result.justification}"
	end

	def determine_result(players)
		result = Result.new
		if players[0].rank.class.rank > players[1].rank.class.rank
			result.winner = players[0]
			result.justification = result.winner.rank.to_s
		elsif players[0].rank.class.rank < players[1].rank.class.rank
			result.winner = players[1]
			result.justification = result.winner.rank.to_s
		else
			case players[0].rank.class.rank
				when 1
					my_sorted_cards = players[0].cards.sort
					other_sorted_cards = players[1].cards.sort

					my_sorted_cards.zip(other_sorted_cards).reverse_each { |e, f| 
						if(e > f)
							result.winner = players[0]
							result.justification = "high card: #{e}"
							break
						elsif (e < f)
							result.winner = players[1]
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