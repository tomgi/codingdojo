class BaseRank
	include Comparable

	def initialize(cards)
		@cards = cards
	end

	def cards
		@cards
	end

	def inspect
		to_s
	end
end