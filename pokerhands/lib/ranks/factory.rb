require 'rank'

class Factory
	def create_rank cards 
		rank = Rank.new
		if (cards.map(&:suit).uniq.size == 1)
			rank.value = 6
			rank.name = "flush"
		else
			rank.value = 1
			rank.name = "high card"
		end
		rank
	end
end