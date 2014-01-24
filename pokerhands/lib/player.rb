require 'card'
require 'rank'

class Player
	attr_accessor :name
	attr_accessor :cards

	def initialize(name = "")
		@name = name
	end

	def rank
		high_card = cards.sort.last
		if (cards.map(&:suit).uniq.size == 1)
			Rank.new Rank::FLUSH, high_card
		else
			
			Rank.new Rank::HIGH_CARD, high_card
		end
	end
end