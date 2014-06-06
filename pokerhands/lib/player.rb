require 'card'
require 'rank'

class Player
	attr_accessor :name
	attr_accessor :cards
	attr_reader :rank

	def initialize(name = "")
		@name = name
	end

	def cards= value
		@cards = value
		@rank = create_rank
	end

	private
	def create_rank 
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