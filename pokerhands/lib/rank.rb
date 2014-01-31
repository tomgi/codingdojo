class Rank
	attr_accessor :value, :name

	def to_s
		name
	end

	def <=> other
		self.value <=> other.value
	end
end