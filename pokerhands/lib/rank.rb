class Rank
	include Comparable

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
		highest_card <=> other.highest_card
	end

	def to_s
		"#{@name.downcase}: #{@highest_card}"
	end

	def inspect
		to_s
	end

end