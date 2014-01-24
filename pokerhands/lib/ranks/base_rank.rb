class BaseRank
	include Comparable



	def initialize(cards)
		@cards = cards
	end

	def <=> other
		self.class.rank <=> other.class.rank
	end

	def cards
		@cards
	end

	def inspect
		to_s
	end
end