class Card
	include Comparable

	def initialize cards_literal
		@cards_literal = cards_literal
	end

	def figure
		@cards_literal[0]
	end

	def suit
		@cards_literal[1]
	end

	def == other
		figure == other.figure && suit == other.suit
	end

	def to_s
		if figure == 'A'
			'Ace'
		else
			figure
		end
	end

	def inspect
		to_s
	end

	def <=> other
		order = '23456789TJQKA'
		(order.index figure) <=> (order.index other.figure)
	end
end