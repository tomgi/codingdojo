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
		Rank.new "High card", high_card
	end
end