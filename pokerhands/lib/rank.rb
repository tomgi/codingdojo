class Rank
	include Comparable

	HIGH_CARD = "high card"
	FLUSH = "flush"

	def initialize rank_name, highest_card
		@name = rank_name
		@highest_card = highest_card
	end

	def name
		@name
	end

	def highest_card
		@highest_card
	end

	def <=>(other)
		order = [HIGH_CARD, FLUSH]
		our = order.index name
		their = order.index other.name 
		if our == their
			highest_card <=> other.highest_card
		else
			our <=> their
		end
	end

	def to_s
		"#{@name}#{name == HIGH_CARD ? ": #{@highest_card}" : ""}"
	end

	def inspect
		to_s
	end

end