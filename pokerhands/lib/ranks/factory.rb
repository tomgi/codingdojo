require 'ranks/high_card'
require 'ranks/flush'

class Factory
	def create_rank cards 
		if (cards.map(&:suit).uniq.size == 1)
			Flush.new cards
		else
			HighCard.new cards
		end
	end
end